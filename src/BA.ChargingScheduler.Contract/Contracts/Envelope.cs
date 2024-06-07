namespace BA.ChargingScheduler.Contract.Contracts
{
    public class Envelope<T>
    {
        public Envelope(T result, string message = null)
        {
            Message = message;
            Result = result;
        }

        public string Message { get; set; }
        public DateTime Timestamp => DateTime.Now;
        public T Result { get; set; }
    }
}
