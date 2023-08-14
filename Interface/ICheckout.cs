using Library.Data.Models;

namespace Library.Interface
{
    public interface ICheckout
    {
        void Add(Checkout newCheckout);

        IEnumerable<Checkout> GetAll();
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);
        IEnumerable<Hold> GetCurrentHolds(int id);
        bool IsCheckedOut(int id);

        Checkout GetById(int checkoutId);
        Checkout GetLatestCheckOut(int assetId);
        string GetCurrentHoldPatronName(int id);
        string GetCurrentCheckoutPatron(int assetId);
        DateTime GetCurrentHoldPlaced(int id);

        void CheckOutItem(int assetId, int checkoutId);
        void CheckInItem(int assetId);
        void PlaceHold(int assetId, int libraryCardId);
        void MarkLost(int assetId);
        void MarkFound(int assetId);

    }
}