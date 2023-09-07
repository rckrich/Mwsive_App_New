public enum ViewID
{
    None = 0,
    //Demo ViewIDs
    PopUpViewModel,
    ErrorViewModel,
    SplashDemoViewModel,
    LogInDemoViewModel,
    MainDemoViewModel,
    HeaderDemoViewModel,
    SettingsDemoViewModel,
    PlaylistViewModel,
    //App ViewIDs
    ProfileViewModel,
    InsigniasViewModel,
    SplashViewModel,
    LogInViewModel,
    MainViewModel,
    MiPlaylistViewModel,
    PlayListViewModel,
    TrackViewModel,
    MenuInferiorViewModel,
    SurfViewModel,
    MenuViewModel,
    CreatePlaylistViewModel,
    OptionsViewModel,
    FollowersViewModel,
    EditProfileViewModel,
    RankingViewModel,
    DangerZoneViewModel,
    ExploreViewModel,
    GenreViewModel,
    TopAlbumViewModel,
    TopArtistViewModel,
    TopCuratorsViewModel,
    TopSongsViewModel,
    TopPlaylistViewModel,
    TopGenreViewModel,
    ChallengesViewModel,
    AddUrlViewModel,
    UserThatVotedViewModel,
}

public enum HTTPMethods
{
    Post,
    Get,
    Put,
    Delete
}

public enum PopUpViewModelTypes
{
    MessageOnly,
    OptionChoice
}

public enum MultipleCallExample
{
    FirstGet = 0,
    SecondGet
}

public enum TrackActionType
{
    NONE = 0,
    PIK,            //Votar canción
    UNPIK,          //Desvotar canción
    RECOMMEND,      //Añadir a playlist
    NOT_RECOMMEND,  //Quitar de playlist
    UP,             //Se movió hacia arriba la canción (Siguiente)
    DOWN            //Se movió hacia arriba la canción (Regresar)
}

public enum MusicalDNAType
{
    NONE = 0,
    ON_REPEAT,              //MIN 1; MAX 1; Type: Track
    ON_REVIVAL,             //MIN 1; MAX 5; Type: Artist
    GUILTY_PLEASURE,        //MIN 1; MAX 5; Type: Track
    ON_LOVE,                //MIN 1; MAX 5; Type: Track
    LATEST_DISCOVERIES,     //MIN 1; MAX 5; Type: Track
    NEXT_STARS,             //MIN 1; MAX 1; Type: Artist
    GOATS,                  //MIN 1; MAX 1; Type: Artist
    OST                     //MIN 4; MAX 18; Type: Track
}

public enum UserLinkType
{
    NONE = 0,
    EXTERNAL,
    TIK_TOK,
    INSTAGRAM,
    YOU_TUBE
}