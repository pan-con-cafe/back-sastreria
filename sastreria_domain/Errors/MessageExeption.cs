namespace sastreria_domain.Errors {
  public class MessageExeption: Exception {
    public MessageExeption(string message)
        : base(message)
    {
    }
  }
}