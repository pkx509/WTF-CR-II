namespace DITS.HILI.WMS.Booking
{
    public class BookingContext
    {

        public void BookingAction(IBookingService booking, params object[] param)
        {
            booking.OnBooking();
        }
    }
}
