namespace web_api
{
    public class NewPlayer
    {
        public string Name { get; set; }

        public NewPlayer(string playerName) {
            Name = playerName;
        }

        public static implicit operator NewPlayer(string player) {
            return new NewPlayer(player);
        }
    }

    // public static class NewPlayerExtensions {
    //     public static
    // }
}
