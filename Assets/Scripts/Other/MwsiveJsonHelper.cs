using System;
using System.Collections.Generic;
using System.Security.Cryptography;

#region Mwsive Entities

public class MwsiveUser : Instanceable
{
    public int id { get; set; }
    public string display_name { get; set; }
    public string email { get; set; }
    public object gender { get; set; }
    public object age { get; set; }
    public string platform { get; set; }
    public object subscription_type { get; set; }
    public string image { get; set; }
    public string platform_id { get; set; }
    public string country { get; set; }
    public int @explicit { get; set; }
    public int platform_followers { get; set; }
    public string public_playlists { get; set; }
    public string? last_selected_playlist { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public int total_followers { get; set; }
    public int total_followed { get; set; }
    public int total_disks { get; set; }
    public List<UserList> user_lists { get; set; }
    public List<UserLink> user_links { get; set; }
}

public class UserList
{
    public int id { get; set; }
    public string type { get; set; }
    public int mwsive_user_id { get; set; }
    public string items { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public List<string> items_list { get; set; }
}

public class UserLink
{
    public int id { get; set; }
    public string type { get; set; }
    public string link { get; set; }
    public int mwsive_user_id { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}

public class Curator
{
    public int id { get; set; }
    public int order { get; set; }
    public int mwsive_user_id { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public MwsiveUser mwsive_user { get; set; }
}

public class MwsiveTrack
{
    public int id { get; set; }
    public string spotify_track_id { get; set; }
    public string spotify_album_id { get; set; }
    public string album_name { get; set; }
    public string spotify_artist_ids { get; set; }
    public string artist_names { get; set; }
    public string available_markets { get; set; }
    public int disk_number { get; set; }
    public int duration_ms { get; set; }
    public string name { get; set; }
    public int popularity { get; set; }
    public string preview_url { get; set; }
    public int track_number { get; set; }
    public string uri { get; set; }
    public int is_local { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public object track { get; set; }
    public Pivot pivot { get; set; }
}

public class MwsivePlaylist
{
    public int id { get; set; }
    public string name { get; set; }
    public int order { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public List<MwsiveTrack> mwsive_tracks { get; set; }
}

public class Pivot
{
    public int playlist_id { get; set; }
    public int mwsive_track_id { get; set; }
}

public class Action
{
    public int id { get; set; }
    public string action { get; set; }
    public MwsiveTrack track { get; set; }
    public MwsiveUser mwsiveUser { get; set; }
    public object created_at { get; set; }
    public object duration { get; set; }
}

public class MusicalDNA
{
    public string type { get; set; }
    public string[] items { get; set; }
}

public class Badge
{
    public int id;
    public string name { get; set; }
    public string type { get; set; }
    public string group { get; set; }
    public string description { get; set; }
    public MwsiveTrack track { get; set; }
}

public class Settings
{
    public int id { get; set; }
    public int name { get; set; }
    public int value { get; set; }
}

public class Challenges
{
    public int id { get; set; }
    public string name { get; set; }
    public int disks { get; set; }
    public int order { get; set; }
    public object open_at { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public List<MwsiveTrack> mwsive_tracks { get; set; }
}

public class Advertising
{
    public int id { get; set; }
    public int order { get; set; }
    public string link { get; set; }
    public string image { get; set; }
    public object deleted_at { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string image_url { get; set; }
}

public class Ranking
{
    public int id { get; set; }
    public MwsiveUser mwsiveUser { get; set; }
    public DateTime week { get; set; }
    public string ranking_group { get; set; }
    public string ranking_subgroup_1 { get; set; }
    public string ranking_subgroup_2 { get; set; }
    public int position { get; set; }
}

public class Pik
{
    public int id { get; set; }
    public Track track { get; set; }
    public MwsiveUser mwsiveUser { get; set; }
    public object created_at { get; set; }
}

public class StreamingStat
{
    public int id { get; set; }
    public MwsiveUser mwsiveUser { get; set; }
    public string type { get; set; }
    public string stat_id { get; set; }
}

public class RecommendedCurator
{
    public string id { get; set; }
    public int order { get; set; }
    public MwsiveUser mwsiveUser { get; set; }
}

public class RecommendedArtist
{
    public string id { get; set; }
    public int order { get; set; }
    public string spotify_id { get; set; }
    public string name { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}

public class RecommendedPlaylist
{
    public int id { get; set; }
    public string name { get; set; }
    public int order { get; set; }
    public string image { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string image_url { get; set; }
    public List<MwsiveTrack> mwsive_tracks { get; set; }
}

public class RecommendedTrack
{
    public string id { get; set; }
    public int order { get; set; }
    public int mwsive_track_id { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public MwsiveTrack mwsive_track { get; set; }
}

public class RecommendedAlbum
{
    public string id { get; set; }
    public int order { get; set; }
    public string spotify_id { get; set; }
    public string name { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}

public class MwsiveGenre
{
    public int id { get; set; }
    public int order { get; set; }
    public int genre_id { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public List<MwsiveTrack> mwsive_tracks { get; set; }
    public Genre genre { get; set; }
}

public class Genre
{
    public int id { get; set; }
    public string name { get; set; }
    public string image { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string image_url { get; set; }
}

#endregion

#region Json Convert Classes

public class MwsiveUserRoot
{
    public MwsiveUser user { get; set; }
}

public class MwsiveCreatedRoot
{
    public MwsiveUser MwsiveUser { get; set; }
}

public class MwsiveLoginRoot
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public MwsiveUser user { get; set; }
}

public class MwsiveCuratorsRoot
{
    public List<MwsiveUser> curators { get; set; } 
}

public class MwsiveFollowersRoot
{
    public List<MwsiveUser> followers { get; set; }
    public int total_follower { get; set; }
}

public class MwsiveFollowedRoot
{
    public List<MwsiveUser> followed { get; set; }
    public int total_folloer { get; set; }
}

public class MwsiveBadgesRoot
{
    public List<Badge> badges { get; set; }
}

public class MwsiveRankingRoot
{
    public List<MwsiveUser> users { get; set; }
}

public class MwsiveSettingsRoot
{
    public List<Settings> settings { get; set; }
}

public class MwsiveChallengesRoot
{
    public List<Challenges> challenges { get; set; }
}

public class MwsiveCompleteChallengesRoot
{
    public int challenge_id { get; set; }
    public int disks { get; set; }
}

public class MwsiveAdvertisingRoot
{
    public List<Advertising> advertisements { get; set; }
}

public class MwsiveRecommendedCuratorsRoot
{
    public List<Curator> curators { get; set; }
}

public class MwsiveRecommendedArtistsRoot
{
    public List<RecommendedArtist> artists { get; set; }
}

public class MwsiveRecommendedPlaylistsRoot
{
    public List<RecommendedPlaylist> playlists { get; set; }
}

public class MwsiveRecommendedTracksRoot
{
    public List<RecommendedTrack> tracks { get; set; }
}

public class MwsiveRecommendedAlbumsRoot
{
    public List<RecommendedAlbum> albums { get; set; }
}

public class MwsiveGenresRoot
{
    public List<MwsiveGenre> genres { get; set; }
}

public class TrackActionRoot
{
    public string track_id { get; set; }
    public string action { get; set; }
    public float duration { get; set; }
}

public class CreateMwsiveUserRoot
{
    public ProfileRoot user { get; set; }
    public string email { get; set; }
    public string access_token { get; set; }
    public string gender { get; set; }
    public int age { get; set; }
    public string[] playlist_ids { get; set; }
}

public class LogInMwsiveRoot
{
    public string email { get; set; }
    public string user_id { get; set; }
}

public class AdvertisementClickRoot
{
    public string user_id { get; set; }
    public string advertisement_id { get; set; }
}

public class PostFollowRoot
{
    public string user_id { get; set; }
}

public class IsFollowingRoot
{
    public bool is_following { get; set; }
}

public class ChallengeCompleteRoot
{
    public int challenge_id { get; set; }
}

public class PostDisplayNameRoot
{
    public string display_name { get; set; }
}

public class PostUserLinkRoot
{
    public string type { get; set; }
    public string url { get; set; }
}

public class TrackInfoRoot
{
    public int total_piks { get; set; }
    public int total_recommendations { get; set; }
    public int? total_piks_followed { get; set; }
    public List<string> top_curators { get; set; }
}

public class PostUserPhoto
{
    public string profile_image { get; set; }
}

#endregion