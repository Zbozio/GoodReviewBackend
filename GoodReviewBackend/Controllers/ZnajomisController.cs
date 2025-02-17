using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodReviewBackend.Models;

namespace GoodReviewBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZnajomisController : ControllerBase
    {
        private readonly GoodReviewDatabaseContext _context;

        public ZnajomisController(GoodReviewDatabaseContext context)
        {
            _context = context;
        }

        // POST: api/Znajomis/SendRequest
        [HttpPost("SendRequest")]
        public async Task<IActionResult> SendFriendRequest(int senderId, int recipientId)
        {
            if (senderId == recipientId)
            {
                return BadRequest("Nie możesz wysłać zaproszenia do siebie.");
            }

            var existingRelation = await _context.Znajomis
                .FirstOrDefaultAsync(z =>
                    (z.IdUzytkownik == senderId && z.UzyIdUzytkownik == recipientId) ||
                    (z.IdUzytkownik == recipientId && z.UzyIdUzytkownik == senderId));

            if (existingRelation != null)
            {
                return BadRequest("Relacja już istnieje lub czeka na akceptację.");
            }

            var newRequest = new Znajomi
            {
                IdUzytkownik = senderId,
                UzyIdUzytkownik = recipientId,
                DataZnajomosci = null,
                StatusZnajomosci = "Pending"
            };

            _context.Znajomis.Add(newRequest);
            await _context.SaveChangesAsync();

            return Ok("Zaproszenie zostało wysłane.");
        }

        // POST: api/Znajomis/RespondToRequest
        [HttpPost("RespondToRequest")]
        public async Task<IActionResult> RespondToFriendRequest(int requestId, bool isAccepted)
        {
            var friendRequest = await _context.Znajomis.FindAsync(requestId);

            if (friendRequest == null)
            {
                return NotFound("Zaproszenie nie zostało znalezione.");
            }

            if (friendRequest.StatusZnajomosci != "Pending")
            {
                return BadRequest("Zaproszenie zostało już zaakceptowane lub odrzucone.");
            }

            if (isAccepted)
            {
                friendRequest.StatusZnajomosci = "Accepted";
                friendRequest.DataZnajomosci = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return Ok("Zaproszenie zostało zaakceptowane.");
            }
            else
            {
                _context.Znajomis.Remove(friendRequest);
                await _context.SaveChangesAsync();
                return Ok("Zaproszenie zostało odrzucone.");
            }
        }

        // GET: api/Znajomis/PendingRequests/5
        [HttpGet("PendingRequests/{userId}")]
        public async Task<IActionResult> GetPendingFriendRequests(int userId)
        {
            var pendingRequests = await _context.Znajomis
                .Where(z =>
                    (z.IdUzytkownik == userId || z.UzyIdUzytkownik == userId) &&
                    z.StatusZnajomosci == "Pending")
                .Select(z => new
                {
                    RequestId = z.IdZnajomosci, // ID zaproszenia
                    SenderId = z.IdUzytkownik == userId ? z.UzyIdUzytkownik : z.IdUzytkownik, // Kto wysłał
                    z.DataZnajomosci,
                    SenderDetails = z.IdUzytkownik == userId
                        ? new
                        {
                            z.UzyIdUzytkownikNavigation.Imie,
                            z.UzyIdUzytkownikNavigation.Nazwisko,
                            z.UzyIdUzytkownikNavigation.Zdjecie
                        }
                        : new
                        {
                            z.IdUzytkownikNavigation.Imie,
                            z.IdUzytkownikNavigation.Nazwisko,
                            z.IdUzytkownikNavigation.Zdjecie
                        }
                })
                .ToListAsync();

            return Ok(pendingRequests);
        }
        // DELETE: api/Znajomis/RemoveFriend/{friendshipId}
        [HttpDelete("RemoveFriend/{friendshipId}")]
        public async Task<IActionResult> RemoveFriend(int friendshipId)
        {
            var friendship = await _context.Znajomis.FindAsync(friendshipId);

            if (friendship == null)
            {
                return NotFound("Znajomość nie została znaleziona.");
            }

            _context.Znajomis.Remove(friendship);
            await _context.SaveChangesAsync();

            return Ok("Znajomość została usunięta.");
        }


        // GET: api/Znajomis/UserFriends/5
        [HttpGet("UserFriends/{userId}")]
        public async Task<IActionResult> GetUserFriends(int userId)
        {
            var friends = await _context.Znajomis
                .Where(z =>
                    (z.IdUzytkownik == userId || z.UzyIdUzytkownik == userId) &&
                    z.StatusZnajomosci == "Accepted")
                .Select(z => new
                {
                    FriendId = z.IdUzytkownik == userId ? z.UzyIdUzytkownik : z.IdUzytkownik,
                    z.DataZnajomosci,
                    z.IdZnajomosci,  // Dodajemy ID znajomości
                    FriendDetails = z.IdUzytkownik == userId
                        ? new
                        {
                            z.UzyIdUzytkownikNavigation.Imie,
                            z.UzyIdUzytkownikNavigation.Nazwisko,
                            z.UzyIdUzytkownikNavigation.EMail,
                            z.UzyIdUzytkownikNavigation.Zdjecie,
                            // Liczba ocenionych książek dla znajomego
                            IloscOcenionychKsiazek = _context.Ocenas
                                .Where(o => o.IdUzytkownik == z.UzyIdUzytkownik)
                                .Select(o => o.IdKsiazka)
                                .Distinct()
                                .Count(),
                            // Liczba napisanych recenzji dla znajomego
                            IloscRecenzji = _context.Recenzjas
                                .Count(r => r.IdUzytkownik == z.UzyIdUzytkownik)
                        }
                        : new
                        {
                            z.IdUzytkownikNavigation.Imie,
                            z.IdUzytkownikNavigation.Nazwisko,
                            z.IdUzytkownikNavigation.EMail,
                            z.IdUzytkownikNavigation.Zdjecie,
                            // Liczba ocenionych książek dla znajomego
                            IloscOcenionychKsiazek = _context.Ocenas
                                .Where(o => o.IdUzytkownik == z.IdUzytkownik)
                                .Select(o => o.IdKsiazka)
                                .Distinct()
                                .Count(),
                            // Liczba napisanych recenzji dla znajomego
                            IloscRecenzji = _context.Recenzjas
                                .Count(r => r.IdUzytkownik == z.IdUzytkownik)
                        }
                })
                .ToListAsync();

            return Ok(friends);
        }
    }
}
