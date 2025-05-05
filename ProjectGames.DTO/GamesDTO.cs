using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace ProjectGames.DTO
{
    public class GamesDTO
    {
        public List<JogosDTO> listGames { get; set; }
        public List<JogosListDTO> listJogosList { get; set; }
        public List<AchievementsDTO> listAchievements { get; set; }
        public List<CategGamesDTO> listCateg { get; set; }
        public List<PercentGameDTO> listRarest { get; set; }
        public List<ScreenshotDTO> listScreen { get; set; }
        public List<FolderGamesDTO> listFolder { get; set; }
        public List<AnotacoesDTO> listNote { get; set; }
        public List<FeedGames> listFeed { get; set; }
        public List<GamesEstatisticas> listEstatistic { get; set; }
        public List<GamesSearch> listSearch { get; set; }
        public List<MusicasDTO> listMusicas { get; set; }
        public MetaGames metaGames { get; set; }
    }

    public class ProfileDTO
    {
        public long id { get; set; }
        public string name { get; set; }
        public string profileUrl { get; set; }
        public string avatar { get; set; }
        public int timeGame { get; set; }
        public int allGames { get; set; }
        public int allAchievements { get; set; }
        public int allPlatinas { get; set; }
        public int perfeitos { get; set; }
        public int porcentagem { get; set; }
    }

    public class JogosDTO
    {
        public long identificador { get; set; }
        public long id { get; set; }
        public string nome { get; set; }
        public string nomeFranquia { get; set; }
        public int ativo { get; set; }
        public string icon { get; set; }
        public string imagem { get; set; }
        public string categoria { get; set; }
        public string descricao { get; set; }
        public int plataforma { get; set; } = 0;
        public DateTime? dtLancamento { get; set; }
        public DateTime? dtObtido { get; set; }
        public string[] screenshots { get; set; }
        public List<ScreenshotDTO> screenshotGame { get; set; }
        public string developer { get; set; }
        public string publisher { get; set; }
        public string requisitos { get; set; }
        public int timeGame { get; set; }
        public string imagemCustom { get; set; }
        public string imgHeader { get; set; }
        public string[] imgGif { get; set; }
        public string banner { get; set; }
        public string about { get; set; }
        public string preco { get; set; }
        public string aux { get; set; } = "";
        public int count { get; set; }
        public int numAllAchievements { get; set; }
        public int numAchievememtUnlock { get; set; }
        public int porcentagem { get; set; }
        public string plataformaG { get; set; }
        public int countSize { get; set; }
        public List<AchievementsDTO> listAchiev { get; set; }
    }

    public class DivPosition
    {
        public string Id { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
    }

    public class JogosListDTO
    {
        public long id { get; set; }
        public string nome { get; set; }
        public int ativo { get; set; }
        public string banner { get; set; }
        public string categoria { get; set; }
        public int favoritos { get; set; }
        public string iconAchievements { get; set; }
        public int timeGame { get; set; }
        public int numAllAchievements { get; set; }
        public int numAchievememtUnlock { get; set; }
        public string iconAchiev { get; set; }
        public string nomeAchiev { get; set; }
        public DateTime dateLastAchievement { get; set; }
        public int porcentagem { get; set; }
        public string ForAchievements { get; set; }
        public string imagem { get; set; }
        public string imgCustom { get; set; }
        public string imgCustomGrid { get; set; }
        public string imgGrid { get; set; }
        public string plataforma { get; set; }
        public int Intplataforma { get; set; }
        public int Indexplataforma { get; set; }
        public int? ordenagem { get; set; }
    }

    public class StaticsGameList
    {
        public int porcentagem { get; set; }
        public int timeGame { get; set; }
        public int allAchievements { get; set; }
        public int allGames { get; set; }
        public int allPlatinas { get; set; }
    }

    public class AchievementsDTO
    {
        public long id { get; set; }
        public long idGame { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public int ativo { get; set; }
        public string iconLock { get; set; }
        public string iconUnlock { get; set; }
        public double porcent { get; set; }
        public DateTime dateUnclock { get; set; }
        public DateTime dateDestaque { get; set; }
    }

    public class CategGamesDTO
    {
        public long id { get; set; }
        public string nome { get; set; }
        public string banner { get; set; }
        public string icon { get; set; }
        public string game { get; set; }
    }

    public class PercentGameDTO
    {
        public long idGame { get; set; }
        public string nomeGame { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public string icon { get; set; }
        public double porcentagem { get; set; }
        public int plataform { get; set; }

    }

    public class ScreenshotDTO
    {
        public long id { get; set; }
        public long idGame { get; set; }
        public string nome { get; set; }
        public string nomeGame { get; set; }
        public string descricao { get; set; }
        public string imagens { get; set; }
        public string icon { get; set; }
        public int favorito { get; set; }
        public int quantidade { get; set; }
        public int idFolder { get; set; }
        public string nomeFolder { get; set; }
        public int plataform { get; set; } = 0;
        public string categoriaGame { get; set; }
    }

    public class FolderGamesDTO
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string imagens { get; set; }
    }

    public class AnotacoesDTO
    {
        public int id { get; set; }
        public int idGame { get; set; }
        public string nome { get; set; }
        public string icon { get; set; }
        public string titulo { get; set; }
        public string mensagem { get; set; }

    }

    public class MetaGames
    {
        public int ano { get; set; }
        public int mes { get; set; }
    }

    public class FeedGames
    {
        public int id { get; set; }
        public int idGame { get; set; }
        public DateTime dateObj { get; set; }
        public string timeElapsed { get; set; }
        public int tipo { get; set; }
        public int plataform { get; set; }
        public string nome { get; set; }
        public string Imagem { get; set; }
        public string imgCustom { get; set; }
        public int time { get; set; }
        public int porcentagem { get; set; }
        public string namePlataform { get; set; }
        public string icon { get; set; }
        public string nomeAch { get; set; }
        public string titulo { get; set; }
        public string mensagem { get; set; }
        public string avatar { get; set; }
        public string nomeProfile { get; set; }
    }

    public class DadosGamesSteam
    {
        public string Nome { get; set; }
        public int Time { get; set; }
        public DateTime DataPlatina { get; set; }
        public string LastAchiev { get; set; }
    }

    public class GamesEstatisticas
    {
        public string imgProfile { get; set; }
        public string nameProfile { get; set; }
        public int plataformInt { get; set; }
        public string plataformName { get; set; }
        public List<LastPlay> listLP { get;set; }
        public List<IndiceGames>listInd { get; set; }
    }

    public class LastPlay
    {
        public int idGame { get; set; }
        public string nome { get; set; }
        public string imagem { get; set; }
        public string imgCustom { get;set; }
        public int progress { get; set; }
        public string imgAchiev { get; set; }
        public string nameAchiev { get; set; }
    }

    public class IndiceGames
    {
        public int year { get; set; }
        public int month { get; set; }
        public string nmMonth { get; set; }
        public int num { get;set; }
    }

    #region API

    #region steam

    public partial class DadosGame
    {
        [JsonProperty("response")]
        public Response Response { get; set; }

        [JsonProperty("playerstats")]
        public PlayerStats playStats { get; set; }

        [JsonProperty("achievementpercentages")]
        public achievementpercentages achievPercent { get; set; }

        [JsonProperty("game")]
        public Games Game { get; set; }
    }

    public partial class Games
    {
        [JsonProperty("availableGameStats")]
        public AvailableGameStats AvailableGameStats { get; set; }
    }

    public partial class AvailableGameStats
    {
        [JsonProperty("achievements")]
        public AchievementAll[] Achievementall { get; set; }
    }

    public partial class PlayerStats
    {
        [JsonProperty("achievements")]
        public Achievement[] Achievements { get; set; }
    }

    public partial class achievementpercentages
    {
        [JsonProperty("achievements")]
        public PercentAchievement[] PercentAchievement { get; set; }
    }

    public partial class AchievementAll
    {
        [JsonProperty("name")]
        public string refName { get; set; }

        [JsonProperty("displayName")]
        public string nome { get; set; }

        [JsonProperty("description")]
        public string descricao { get; set; } = "";

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("icongray")]
        public string IconLock { get; set; }

        public string Obtido { get; set; }

        public long DateUnlock { get; set; }

        public double porcent { get; set; }
    }

    public partial class Achievement
    {
        [JsonProperty("apiname")]
        public string name { get; set; }

        [JsonProperty("achieved")]
        public long? Achieved { get; set; }

        [JsonProperty("unlocktime")]
        public long date { get; set; }
    }

    public partial class PercentAchievement
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("percent")]
        public double porcentual { get; set; }
    }

    public partial class Response
    {
        [JsonProperty("total_count")]
        public long TotalCount { get; set; }

        [JsonProperty("games")]
        public Game[] Games { get; set; }
    }

    public partial class Game
    {
        [JsonProperty("appid")]
        public long Appid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("playtime_forever")]
        public long time { get; set; }

        [JsonProperty("img_icon_url")]
        public string ImgIconUrl { get; set; }
    }


    public class GamesLstDesejo
    {
        public long id { get; set; }
        public string Name { get; set; }
        public string image { get; set; }
        public long ReleaseDate { get; set; }
        public string Review { get; set; }
    }

    public class SteamWishlistData
    {
        public string AppId { get; set; }
        public string Name { get; set; }
        public string Imagem { get; set; }
        public long Date { get; set; }
        public string Review { get; set; }
    }

    public class rarestAchievements
    {
        public string name { get; set; }

    }

    public class GameDetails
    {
        public string AppId { get; set; }
        public string BackgroundRaw { get; set; }
        public string descricao { get; set; }
        public string pc { get; set; }
        public int metaCritic { get; set; }

    }

    public class PcRequirements
    {
        public string Minimum { get; set; }
        public string Recommended { get; set; }
    }

    public class Screenshot
    {
        public int Id { get; set; }
        public string PathThumbnail { get; set; }
        public string PathFull { get; set; }
    }

    public class Recommendations
    {
        public int Total { get; set; }
    }

    public class Achievements
    {
        public int Total { get; set; }
        public List<AchievementTeste> Highlighted { get; set; }
    }

    public class AchievementTeste
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    #endregion

    #region Retro

    public partial class DadosRetro
    {
        [JsonProperty("Results")]
        public GamesRetro[] Results { get; set; }

        [JsonProperty("ImageBoxArt")]
        public string imageBox { get; set; }

        [JsonProperty("Publisher")]
        public string Publisher { get; set; }

        [JsonProperty("Developer")]
        public string Developer { get; set; }

        [JsonProperty("NumDistinctPlayers")]
        public double NumDistinctPlayers { get; set; }

        [JsonProperty("Released")]
        public DateTime? dtReleased { get; set; } = null;

        [JsonProperty("Achievements")]
        public Dictionary<string, AchievementRetro> Achievements { get; set; }
    }

    public class ProfileRetro
    {
        public string User { get; set; }
        public string UserPic { get; set; }
        public int ID { get; set; }
    }

    public partial class GamesRetro
    {
        public int GameID { get; set; }
        public string Title { get; set; }
        public string ImageIcon { get; set; }
        public int ConsoleID { get; set; }
        public string ConsoleName { get; set; }
        public int MaxPossible { get; set; }
        public int NumAwarded { get; set; }
        public int NumAwardedHardcore { get; set; }
        public DateTime MostRecentAwardedDate { get; set; }
        public string HighestAwardKind { get; set; }
        public DateTime? HighestAwardDate { get; set; }
    }

    public  partial class AchievementRetro
    {
        public long Id { get; set; }
        public double NumAwarded { get; set; }
        public double NumAwardedHardcore { get; set; }
        public double porcentagemAch { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BadgeName { get; set; }
        public DateTime? DateEarned { get; set; } = null;
    }

    #endregion

    #region ImageGrid

    public partial class GridGames
    {
        [JsonProperty("data")]
        public imgGrid[] listImgGrid { get; set; }
    }

    public partial class imgGrid
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("thumb")]
        public string imagem { get; set; }
        [JsonProperty("url")]
        public string banner { get; set; }
        [JsonProperty("width")]
        public string width { get; set; }
        [JsonProperty("height")]
        public string height { get; set; }
    }

    public partial class GamesSearch
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string imagem { get; set; }
        public string console { get; set; }
        public int  plataforma { get;set; }
    }
    #endregion
    #endregion

    public class MusicasDTO
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string sound { get; set; }
    }
}
