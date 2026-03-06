
public class Message 
{
    public int Id { get; set; }//номер сообщения
    public bool IsMedia { get; set; }//какой тип сообщения, медиа или текст

    public Message (int id, bool isMedia) {  Id = id; IsMedia = isMedia; }

}