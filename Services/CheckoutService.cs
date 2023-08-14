using System;
using Library.Data.Models;
using Library.Interface;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class CheckoutService : ICheckout
    {
        private readonly LibraryDbContext _context;

        public CheckoutService(LibraryDbContext context)
        {
            this._context = context;
        }
        public void Add(Checkout newCheckout)
        {
            _context.Add(newCheckout);
            _context.SaveChanges();
        }


        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts;
        }

        public Checkout GetById(int checkoutId)
        {
            return GetAll()
                 .FirstOrDefault(checkout => checkout.Id == checkoutId);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {
            return _context.CheckoutHistories
                    .Include(h => h.LibraryAsset)
                    .Include(h => h.LibraryCard)
                    .Where(history => history.LibraryAsset.Id == id);
        }


        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Holds
                    .Include(h => h.LibraryAsset)
                    .Include(h => h.LibraryCard)
                    .Where(h => h.LibraryAsset.Id == id);
        }


        public void MarkFound(int assetId)
        {
            var now = DateTime.Now;

            UpdateAsssetStatus(assetId, "Available");
            RemoveExisitingCheckouts(assetId);
            CloseExistingCheckoutHistory(assetId, now);


            _context.SaveChanges();
        }

        private void UpdateAsssetStatus(int assetId, string newStatus)
        {
            var item = _context.LibraryAssets
                 .FirstOrDefault(a => a.Id == assetId);

            _context.Update(item);
            item.Status = _context.Statuses
                                .FirstOrDefault(status => status.Name == newStatus);
        }

        private void CloseExistingCheckoutHistory(int assetId, DateTime now)
        {
            // close any existing checkout history 
            var history = _context.CheckoutHistories
                            .FirstOrDefault(h => h.LibraryAsset.Id == assetId
                                && h.CheckedIn == null);
            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = now;
            }
        }

        private void RemoveExisitingCheckouts(int assetId)
        {
            // if item is found remove any other checkouts from that item 
            var checkout = _context.Checkouts
                            .FirstOrDefault(co => co.LibraryAsset.Id == assetId);
            Console.WriteLine(checkout);
            if (checkout != null)
            {
                _context.Remove(checkout);
            }
        }

        public void MarkLost(int assetId)
        {
            UpdateAsssetStatus(assetId, "Lost");

            _context.SaveChanges();
        }

        public void CheckInItem(int assetId)
        {
            var now = DateTime.Now;

            var item = _context.LibraryAssets.FirstOrDefault(a => a.Id == assetId);

            _context.Update(item);

            // remove any exisiting checkouts on the item
            RemoveExisitingCheckouts(assetId);

            // close any existing checkout history
            CloseExistingCheckoutHistory(assetId, now);

            // look for any existing holds on the item
            var currentHolds = _context.Holds
                            .Include(h => h.LibraryAsset)
                            .Include(h => h.LibraryCard)
                            .Where(h => h.LibraryAsset.Id == assetId);
            // if there is hold, checkout the item to the library card with the earliest hold
            if (currentHolds.Any())
            {
                CheckoutToEarliestHold(assetId, currentHolds);
                return;
            }
            // otherwise, update the item status to "available"
            UpdateAsssetStatus(assetId, "Available");
            _context.SaveChanges();

        }

        private void CheckoutToEarliestHold(int assetId, IQueryable<Hold> currentHolds)
        {
            var earliestHold = currentHolds
                                .OrderBy(Hold => Hold.HoldPlaced)
                                .FirstOrDefault();
            var card = earliestHold.LibraryCard;

            _context.Remove(earliestHold);
            _context.SaveChanges();
            CheckOutItem(assetId, card.Id);
        }

        public void CheckOutItem(int assetId, int libraryCardId)
        {
            if (IsCheckedOut(assetId))
            {
                return;
                // Add logic to handle feedback to user 
            }
            var item = _context.LibraryAssets
                .Include(a => a.Status)
                .FirstOrDefault(a => a.Id == assetId);

            _context.Update(item);
            UpdateAsssetStatus(assetId, "Checked Out");

            var libraryCard = _context.LibraryCards
                    .Include(card => card.Checkouts)
                    .FirstOrDefault(card => card.Id == libraryCardId);

            var now = DateTime.Now;
            var checkout = new Checkout
            {
                LibraryAsset = item,
                LibraryCard = libraryCard,
                Since = now,
                Until = GetDefaultCheckoutTime(now)
            };

            _context.Add(checkout);

            var checkoutHistory = new CheckoutHistory
            {
                CheckedOut = now,
                LibraryAsset = item,
                LibraryCard = libraryCard,
            };

            _context.Add(checkoutHistory);
            _context.SaveChanges();
        }

        private DateTime GetDefaultCheckoutTime(DateTime now)
        {
            return now.AddDays(30);
        }

        public bool IsCheckedOut(int assetId)
        {
            var isCheckedOut = _context.Checkouts
                    .Where(co => co.LibraryAsset.Id == assetId)
                    .Any();
            return isCheckedOut;
        }

        public void PlaceHold(int assetId, int libraryCardId)
        {
            var now = DateTime.Now;
            var asset = _context.LibraryAssets
                    .Include(a => a.Status)
                    .FirstOrDefault(a => a.Id == assetId);

            var card = _context.LibraryCards
                        .FirstOrDefault(c => c.Id == libraryCardId);

            if (asset.Status.Name == "Available")
            {
                UpdateAsssetStatus(assetId, "On Hold");
            }

            var hold = new Hold
            {
                HoldPlaced = now,
                LibraryAsset = asset,
                LibraryCard = card
            };
            _context.Add(hold);
            _context.SaveChanges();
        }
        public string GetCurrentHoldPatronName(int holdId)
        {
            var hold = _context.Holds
                    .Include(h => h.LibraryAsset)
                    .Include(h => h.LibraryCard)
                    .FirstOrDefault(h => h.Id == holdId);


            var cardId = hold?.LibraryCard.Id;
            var patron = _context.Patrons
                    .Include(p => p.LibraryCard)
                    .FirstOrDefault(p => p.LibraryCard.Id == cardId);

            return patron?.FirstName + " " + patron?.LastName;
        }
        public DateTime GetCurrentHoldPlaced(int holdId)
        {

            var holdPlaced = _context.Holds
                    .Include(h => h.LibraryAsset)
                    .Include(h => h.LibraryCard)
                    .FirstOrDefault(h => h.Id == holdId)
                    .HoldPlaced;
            return holdPlaced;
        }

        public Checkout GetLatestCheckOut(int assetId)
        {
            return _context.Checkouts
                .Where(c => c.LibraryAsset.Id == assetId)
                .OrderByDescending(c => c.Since)
                .FirstOrDefault();
        }

        public string GetCurrentCheckoutPatron(int assetId)
        {
            var checkout = GetCheckoutByAssetId(assetId);
            if (checkout == null)
            {
                return "";
            }
            var cardId = checkout.LibraryCard.Id;

            var patron = _context.Patrons
                    .Include(p => p.LibraryCard)
                    .FirstOrDefault(p => p.LibraryCard.Id == cardId);

            return patron.FirstName + " " + patron.LastName;
        }

        private Checkout GetCheckoutByAssetId(int assetId)
        {
            var checkout = _context.Checkouts
                .Include(c => c.LibraryAsset)
                .Include(c => c.LibraryCard)
                .FirstOrDefault(c => c.LibraryAsset.Id == assetId);
            return checkout;
        }

    }
}