namespace Movie_Rating.Models.DTO
{
    public class encrypt
    {
       public  string EncryptedToken { get; set; }
       public string TagBase64 { get; set; }
        public string NonceBase64 { get; set; }
    }
}
