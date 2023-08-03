using System;
using System.Collections.Generic;

#region Spotify Entities

public class ExplicitContent
{
    public bool filter_enabled { get; set; }
    public bool filter_locked { get; set; }
}

public class ExternalIds
{
    public string isrc { get; set; }
    public string ean { get; set; }
    public string upc { get; set; }
}

public class ExternalUrls
{
    public string spotify { get; set; }
}

public class Followers
{
    public object href { get; set; }
    public int total { get; set; }
}

public class Copyright
{
    public string text { get; set; }
    public string type { get; set; }
}

public class VideoThumbnail
{
    public object url { get; set; }
}

public class Author
{
    public string name { get; set; }
}

public class Seed
{
    public int initialPoolSize { get; set; }
    public int afterFilteringSize { get; set; }
    public int afterRelinkingSize { get; set; }
    public string id { get; set; }
    public string type { get; set; }
    public string href { get; set; }
}

public class AddedBy
{
    public ExternalUrls external_urls { get; set; }
    public Followers followers { get; set; }
    public string href { get; set; }
    public string id { get; set; }
    public string type { get; set; }
    public string uri { get; set; }
}

public class SpotifyImage
{
    public object height { get; set; }
    public string url { get; set; }
    public object width { get; set; }
}

public class ProfileRoot
{
    public string country { get; set; }
    public string display_name { get; set; }
    public string email { get; set; }
    public ExplicitContent explicit_content { get; set; }
    public ExternalUrls external_urls { get; set; }
    public Followers followers { get; set; }
    public string href { get; set; }
    public string id { get; set; }
    public List<SpotifyImage> images { get; set; }
    public string product { get; set; }
    public string type { get; set; }
    public string uri { get; set; }
}

public class Album
{
    public string album_type { get; set; }
    public int total_tracks { get; set; }
    public List<string> available_markets { get; set; }
    public ExternalUrls external_urls { get; set; }
    public string href { get; set; }
    public string id { get; set; }
    public List<SpotifyImage> images { get; set; }
    public string name { get; set; }
    public string release_date { get; set; }
    public string release_date_precision { get; set; }
    public Restrictions restrictions { get; set; }
    public string type { get; set; }
    public string uri { get; set; }
    public List<Copyright> copyrights { get; set; }
    public ExternalIds external_ids { get; set; }
    public List<string> genres { get; set; }
    public string label { get; set; }
    public int popularity { get; set; }
    public string album_group { get; set; }
    public List<Artist> artists { get; set; }
}

public class Albums
{
    public string href { get; set; }
    public int limit { get; set; }
    public string next { get; set; }
    public int offset { get; set; }
    public string previous { get; set; }
    public int total { get; set; }
    public List<Item> items { get; set; }
}

public class Artist
{
    public ExternalUrls external_urls { get; set; }
    public string href { get; set; }
    public string id { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public string uri { get; set; }
    public Followers followers { get; set; }
    public List<string> genres { get; set; }
    public List<SpotifyImage> images { get; set; }
    public int popularity { get; set; }
    public int limit { get; set; }
    public string next { get; set; }
    public int offset { get; set; }
    public string previous { get; set; }
    public int total { get; set; }
    public List<Item> items { get; set; }
}

public class Artists
{
    public string href { get; set; }
    public List<Item> items { get; set; }
    public int limit { get; set; }
    public string next { get; set; }
    public int offset { get; set; }
    public string previous { get; set; }
    public int total { get; set; }
}

public class Audiobooks
{
    public string href { get; set; }
    public int limit { get; set; }
    public string next { get; set; }
    public int offset { get; set; }
    public string previous { get; set; }
    public int total { get; set; }
    public List<Item> items { get; set; }
}

public class Episodes
{
    public string href { get; set; }
    public int limit { get; set; }
    public string next { get; set; }
    public int offset { get; set; }
    public string previous { get; set; }
    public int total { get; set; }
    public List<Item> items { get; set; }
}

public class Playlists
{
    public string href { get; set; }
    public int limit { get; set; }
    public string next { get; set; }
    public int offset { get; set; }
    public string previous { get; set; }
    public int total { get; set; }
    public List<Item> items { get; set; }
}

public class LinkedFrom
{
    public ExternalUrls external_urls { get; set; }
    public string href { get; set; }
    public string id { get; set; }
    public string type { get; set; }
    public string uri { get; set; }
}

public class Restrictions
{
    public string reason { get; set; }
}

public class ResumePoint
{
    public bool fully_played { get; set; }
    public int resume_position_ms { get; set; }
}

public class Narrator
{
    public string name { get; set; }
}

public class Owner
{
    public ExternalUrls external_urls { get; set; }
    public Followers followers { get; set; }
    public string href { get; set; }
    public string id { get; set; }
    public string type { get; set; }
    public string uri { get; set; }
    public string display_name { get; set; }
}

public class Shows
{
    public string href { get; set; }
    public int limit { get; set; }
    public string next { get; set; }
    public int offset { get; set; }
    public string previous { get; set; }
    public int total { get; set; }
    public List<Item> items { get; set; }
}

public class Tracks
{
    public string href { get; set; }
    public int limit { get; set; }
    public string next { get; set; }
    public int offset { get; set; }
    public string previous { get; set; }
    public int total { get; set; }
    public List<Item> items { get; set; }
}

public class Track
{
    public Album album { get; set; }
    public List<Artist> artists { get; set; }
    public List<string> available_markets { get; set; }
    public int disc_number { get; set; }
    public int duration_ms { get; set; }
    public bool @explicit { get; set; }
    public ExternalIds external_ids { get; set; }
    public ExternalUrls external_urls { get; set; }
    public string href { get; set; }
    public string id { get; set; }
    public bool is_playable { get; set; }
    public LinkedFrom linked_from { get; set; }
    public Restrictions restrictions { get; set; }
    public string name { get; set; }
    public int popularity { get; set; }
    public string preview_url { get; set; }
    public int track_number { get; set; }
    public string type { get; set; }
    public string uri { get; set; }
    public bool is_local { get; set; }
}

public class Item
{
    public bool collaborative { get; set; }
    public string description { get; set; }
    public Album album { get; set; }
    public List<Artist> artists { get; set; }
    public List<string> available_markets { get; set; }
    public int disc_number { get; set; }
    public int duration_ms { get; set; }
    public bool @explicit { get; set; }
    public ExternalIds external_ids { get; set; }
    public ExternalUrls external_urls { get; set; }
    public Followers followers { get; set; }
    public List<string> genres { get; set; }
    public string href { get; set; }
    public string id { get; set; }
    public List<SpotifyImage> images { get; set; }
    public string name { get; set; }
    public Owner owner { get; set; }
    public bool @public { get; set; }
    public string snapshot_id { get; set; }
    public Tracks tracks { get; set; }
    public object primary_color { get; set; }
    public int popularity { get; set; }
    public string preview_url { get; set; }
    public int track_number { get; set; }
    public string type { get; set; }
    public string uri { get; set; }
    public bool is_local { get; set; }
    public string added_at { get; set; }
    public AddedBy added_by { get; set; }
    public Track track { get; set; }
    public bool is_playable { get; set; }
    public LinkedFrom linked_from { get; set; }
    public Restrictions restrictions { get; set; }
    public string album_type { get; set; }
    public int total_tracks { get; set; }
    public string release_date { get; set; }
    public string release_date_precision { get; set; }
    public List<Copyright> copyrights { get; set; }
    public string label { get; set; }
    public string album_group { get; set; }
    public string html_description { get; set; }
    public bool is_externally_hosted { get; set; }
    public List<string> languages { get; set; }
    public string media_type { get; set; }
    public string publisher { get; set; }
    public int total_episodes { get; set; }
    public string audio_preview_url { get; set; }
    public string language { get; set; }
    public ResumePoint resume_point { get; set; }
    public List<Author> authors { get; set; }
    public string edition { get; set; }
    public List<Narrator> narrators { get; set; }
    public int total_chapters { get; set; }
    public VideoThumbnail video_thumbnail { get; set; }
}

#endregion

#region Json Convert Classes

[System.Serializable]
public class CreatePlaylistBodyRequestRoot
{
    public string name { get; set; }
    public string description { get; set; }
    public bool @public { get; set; }
}

[System.Serializable]
public class EditPlaylistCompleteBodyRequestRoot
{
    public string name { get; set; }
    public string description { get; set; }
    public bool @public { get; set; }
}

[System.Serializable]
public class EditPlaylistNoDescriptionBodyRequestRoot
{
    public string name { get; set; }
    public bool @public { get; set; }
}

[System.Serializable]
public class AddItemsPlaylistBodyRequestRoot
{
    public List<string> uris { get; set; }
    public int position { get; set; }
}

[System.Serializable]
public class RemoveItemsPlaylistBodyRequestRoot
{
    public List<RemoveTrack> tracks { get; set; }
}

[System.Serializable]
public class RemoveTrack
{
    public string uri { get; set; }
}

#endregion

#region Root Classes

[System.Serializable]
public class UserTopItemsRoot
{
    public List<Item> items { get; set; }
    public int total { get; set; }
    public int limit { get; set; }
    public int offset { get; set; }
    public string href { get; set; }
    public string next { get; set; }
    public object previous { get; set; }
}

[System.Serializable]
public class TrackRoot
{
    public Album album { get; set; }
    public List<Artist> artists { get; set; }
    public List<string> available_markets { get; set; }
    public int disc_number { get; set; }
    public int duration_ms { get; set; }
    public bool @explicit { get; set; }
    public ExternalIds external_ids { get; set; }
    public ExternalUrls external_urls { get; set; }
    public string href { get; set; }
    public string id { get; set; }
    public bool is_playable { get; set; }
    public LinkedFrom linked_from { get; set; }
    public Restrictions restrictions { get; set; }
    public string name { get; set; }
    public int popularity { get; set; }
    public string preview_url { get; set; }
    public int track_number { get; set; }
    public string type { get; set; }
    public string uri { get; set; }
    public bool is_local { get; set; }
}

[System.Serializable]
public class PlaylistRoot
{
    public string href { get; set; }
    public int limit { get; set; }
    public object next { get; set; }
    public int offset { get; set; }
    public object previous { get; set; }
    public int total { get; set; }
    public List<Item> items { get; set; }
}

[System.Serializable]
public class SearchedPlaylist
{
    public bool collaborative { get; set; }
    public string description { get; set; }
    public ExternalUrls external_urls { get; set; }
    public Followers followers { get; set; }
    public string href { get; set; }
    public string id { get; set; }
    public List<SpotifyImage> images { get; set; }
    public string name { get; set; }
    public Owner owner { get; set; }
    public bool @public { get; set; }
    public string snapshot_id { get; set; }
    public Tracks tracks { get; set; }
    public string type { get; set; }
    public string uri { get; set; }
}

[System.Serializable]
public class CreatedPlaylistRoot
{
    public bool collaborative { get; set; }
    public string description { get; set; }
    public ExternalUrls external_urls { get; set; }
    public Followers followers { get; set; }
    public string href { get; set; }
    public string id { get; set; }
    public List<SpotifyImage> images { get; set; }
    public string name { get; set; }
    public Owner owner { get; set; }
    public bool @public { get; set; }
    public string snapshot_id { get; set; }
    public Tracks tracks { get; set; }
    public string type { get; set; }
    public string uri { get; set; }
}

public class SeveralTrackRoot
{
    public List<Track> tracks { get; set; }
}

[System.Serializable]
public class AddItemsToPlaylistRoot
{
    public string snapshot_id { get; set; }
}

[System.Serializable]
public class RemoveItemsToPlaylistRoot
{
    public string snapshot_id { get; set; }
}

[System.Serializable]
public class RecommendationsRoot
{
    public List<Track> tracks { get; set; }
    public List<Seed> seeds { get; set; }
}

[System.Serializable]
public class SearchRoot
{
    public Albums albums { get; set; }
    public Artists artists { get; set; }
    public Tracks tracks { get; set; }
    public Playlists playlists { get; set; }
    public Shows shows { get; set; }
    public Episodes episodes { get; set; }
    public Audiobooks audiobooks { get; set; }
}

[System.Serializable]
public class GenresRoot
{
    public List<string> genres { get; set; }
}

#endregion