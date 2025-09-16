namespace Hospital.Core.Helpers
{
    public class GenericResponse<T>
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }
    }
}
