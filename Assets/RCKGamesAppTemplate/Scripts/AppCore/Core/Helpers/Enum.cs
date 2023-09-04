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
    ChallengesViewModel
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