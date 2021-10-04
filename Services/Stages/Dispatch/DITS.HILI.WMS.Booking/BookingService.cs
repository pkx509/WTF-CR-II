namespace DITS.HILI.WMS.Booking
{
    public class BookingService
    {

        public void OnBooking(params object[] param)
        {
            //Load Configuration

            bool ok = true;
            IBookingService method;

            if (ok)
            {
                method = new BookingMethodA();
            }
            else
            {
                method = new BookingMethodB();
            }

            BookingContext Context = new BookingContext();
            Context.BookingAction(method, param);

        }
    }
}
