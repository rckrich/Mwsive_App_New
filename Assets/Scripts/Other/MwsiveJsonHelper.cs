using System;
using System.Collections.Generic;

#region Mwsive Entities

public class MwsiveUser
{
    public int id { get; set; }
    public string display_name { get; set; }
    public string email { get; set; }
    public string platform { get; set; }
    public string subscription_type { get; set; }
    public bool @explicit { get; set; }
    public string country { get; set; }
    public string image { get; set; }
    public Ranking[] rankings { get; set; }
    public Badge[] badges { get; set; }
    public Action[] actions { get; set; }
    public Pik[] piks { get; set; }
    public int followers { get; set; }
    public int following { get; set; }
    public int platform_followers { get; set; }
    public Track[] on_repeat { get; set; }
    public Track[] on_revival { get; set; }
    public Track[] guilty_pleasures { get; set; }
    public Track[] loved { get; set; }
    public Track[] discoveries { get; set; }
    public Track[] next_starts { get; set; }
    public Artist[] goats { get; set; }
    public Track[] ost { get; set; }
    public object created_at { get; set; }
    public User user { get; set; }
    public Playlists[] publicPlaylists { get; set; }
    public string genre { get; set; }
    public int age { get; set; }
}

public class MwsiveTrack
{
    public int id { get; set; }
    public Album album { get; set; }
    public string album_name { get; set; }
    public string[] artists_id { get; set; }
    public string[] artists_name { get; set; }
    public string[] available_markets { get; set; }
    public int disk_number { get; set; }
    public int duration_ms { get; set; }
    public string track_id { get; set; }
    public bool is_playable { get; set; }
    public object restrictions { get; set; }
    public string name { get; set; }
    public int popularity { get; set; }
    public string preview_url { get; set; }
    public int track_number { get; set; }
    public string type { get; set; }
    public string uri { get; set; }
    public bool is_local { get; set; }
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
    public List<string> track_ids { get; set; }
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
    public MwsiveTrack[] tracks { get; set; }
    public object open_at { get; set; }
}

public class Advertising
{
    public string link { get; set; }
    public Media media { get; set; }
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

public class UserLink
{
    public int id { get; set; }
    public MwsiveUser mwsiveUser { get; set; }
    public string type { get; set; }
    public string link { get; set; }
}

public class RecommendedCurator
{
    public int id { get; set; }
    public int order { get; set; }
    public MwsiveUser mwsiveUser { get; set; }
}

public class RecommendedArtist
{
    public int id { get; set; }
    public int order { get; set; }
    public Artist artist { get; set; }
}

public class RecommendedPlaylist
{
    public int id { get; set; }
    public string name { get; set; }
    public int order { get; set; }
    public MwsiveTrack[] tracks { get; set; }
}

public class RecommendedTrack
{
    public int id { get; set; }
    public int order { get; set; }
    public Track track { get; set; }
}

public class RecommendedAlbum
{
    public int id { get; set; }
    public int order { get; set; }
    public Album album { get; set; }
}

public class Genre
{
    public int id { get; set; }
    public string name { get; set; }
    public int order { get; set; }
    public MwsiveTrack[] tracks { get; set; }
}

#endregion

#region Json Convert Classes

public class MwsiveUserRoot
{
    public string MwsiveUser { get; set; }
}

public class MwsiveCreatenRoot
{
    public MwsiveUser MwsiveUser { get; set; }
    public string mwsive_token { get; set; }
}

public class MwsiveLoginRoot
{
    public MwsiveUser MwsiveUser { get; set; }
    public string mwsive_token { get; set; }
}

public class MwsiveCuratorsRoot
{
    public List<MwsiveUser> curators { get; set; } 
}

public class MwsiveFollowersRoot
{
    public List<MwsiveUser> followers { get; set; }
}

public class MwsiveFollowingRoot
{
    public List<MwsiveUser> followers { get; set; }
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
    public int points_to_add { get; set; }
}

public class MwsiveAdvertisingRoot
{
    public List<Advertising> advertising { get; set; }
}

public class MwsiveRecommendedCuratorsRoot
{
    public List<MwsiveUser> users { get; set; }
}

public class MwsiveRecommendedArtistsRoot
{
    public List<RecommendedArtist> recommended_artists { get; set; }
}

public class MwsiveRecommendedPlaylistsRoot
{
    public List<RecommendedPlaylist> recommended_playlists { get; set; }
}

public class MwsiveRecommendedTracksRoot
{
    public List<RecommendedTrack> recommended_tracks { get; set; }
}

public class MwsiveRecommendedAlbumsRoot
{
    public List<RecommendedAlbum> recommended_albums { get; set; }
}

public class MwsiveGenresRoot
{
    public List<Genre> genres { get; set; }
}

public class TrackActionRoot
{
    public int user_id { get; set; }
    public int track_id { get; set; }
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

#endregion