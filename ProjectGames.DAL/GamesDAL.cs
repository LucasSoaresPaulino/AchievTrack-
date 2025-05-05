using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generico;
using ProjectGames.DTO;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Intrinsics.X86;
using System.IO;
using System.Reflection;
using static System.Net.WebRequestMethods;

namespace ProjectGames.DAL
{
    public class GamesDAL
    {
        string connectString = Conexao.Conectar();

        public void spInsGames(DadosGame listGames)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                for (int i = 0; i <= listGames.Response.Games.Length - 1; i++)
                {
                    TimeSpan spWorkMin = TimeSpan.FromMinutes(listGames.Response.Games[i].time);
                    var workHours = Convert.ToInt32(spWorkMin.TotalHours);
                    using (SqlCommand cmd = new SqlCommand("spInsGamesSteam", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idGame", listGames.Response.Games[i].Appid);
                        cmd.Parameters.AddWithValue("@Nome", listGames.Response.Games[i].Name);
                        cmd.Parameters.AddWithValue("@iconUrl", "https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/apps/" + listGames.Response.Games[i].Appid + "/" + listGames.Response.Games[i].ImgIconUrl + ".jpg");
                        cmd.Parameters.AddWithValue("@ImgUrl", "https://cdn.cloudflare.steamstatic.com/steam/apps/" + listGames.Response.Games[i].Appid + "/hero_capsule.jpg");
                        cmd.Parameters.AddWithValue("@time", Convert.ToInt32(workHours));
                        cmd.Parameters.AddWithValue("@ativo", 1);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void spInsRetroGames(DadosRetro listRetro, int type, int? idGame)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                if (type == 0)
                {
                    foreach (var item in listRetro.Results)
                    {
                        using (SqlCommand cmd = new SqlCommand("spInsRetroGames", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@idGame", item.GameID);
                            cmd.Parameters.AddWithValue("@nome", item.Title);
                            cmd.Parameters.AddWithValue("@icon", item.ImageIcon);
                            cmd.Parameters.AddWithValue("@allAchiev", item.MaxPossible);
                            cmd.Parameters.AddWithValue("@myAchiev", item.NumAwarded);
                            cmd.Parameters.AddWithValue("@consoleId", item.ConsoleID);
                            cmd.Parameters.AddWithValue("@dateLastAch", item.MostRecentAwardedDate);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                if (type == 1)
                {
                    using (SqlCommand cmd = new SqlCommand("spUpdDetailsRetroGames", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idGame", idGame);
                        cmd.Parameters.AddWithValue("@imagem", listRetro.imageBox);
                        cmd.Parameters.AddWithValue("@developer", listRetro.Developer);
                        cmd.Parameters.AddWithValue("@publiser", listRetro.Publisher);
                        cmd.Parameters.AddWithValue("@dateReleased", listRetro.dtReleased);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void spInsAchievementsRetroGames(DadosRetro listAchiev, int idGame)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                foreach (var item in listAchiev.Achievements)
                {
                    using (SqlCommand cmd = new SqlCommand("spInsAchievementsRetroGames", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idGame", idGame);
                        cmd.Parameters.AddWithValue("@idAchievement", item.Value.Id);
                        cmd.Parameters.AddWithValue("@titulo", item.Value.Title);
                        cmd.Parameters.AddWithValue("@descricao", item.Value.Description);
                        cmd.Parameters.AddWithValue("@icon", item.Value.BadgeName);
                        if (item.Value.DateEarned != null)
                        {
                            cmd.Parameters.AddWithValue("@ativo", 1);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ativo", 0);
                        }
                        cmd.Parameters.AddWithValue("@dataObtido", item.Value.DateEarned);
                        cmd.Parameters.AddWithValue("@porcentagem", item.Value.porcentagemAch);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void spInsProfileRetro(ProfileRetro profile)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsProfileRetro", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUser", profile.ID);
                    cmd.Parameters.AddWithValue("@usuario", profile.User);
                    cmd.Parameters.AddWithValue("@imagem", profile.UserPic);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spInsPercentualRetroGames()
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsPercentualRetroGames", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spInsGamesListDesejo(List<SteamWishlistData> listDsj)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                for (int i = 0; i < listDsj.Count; i++)
                {
                    using (SqlCommand cmd = new SqlCommand("spInsGamesListDesejo", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@idGame", listDsj[i].AppId);
                        cmd.Parameters.AddWithValue("@nome", listDsj[i].Name);
                        cmd.Parameters.AddWithValue("@imagem", listDsj[i].Imagem);
                        cmd.Parameters.AddWithValue("@review", listDsj[i].Review);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void spInsAchievementsGames(AchievementAll[] listAchievements, int id)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                for (int i = 0; i <= listAchievements.Length - 1; i++)
                {
                    long timestmp = listAchievements[i].DateUnlock;
                    System.DateTime dat_Time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                    dat_Time = dat_Time.AddSeconds(timestmp);
                    string data = dat_Time.ToShortDateString() + " " + dat_Time.ToShortTimeString();

                    using (SqlCommand cmd = new SqlCommand("spInsAchievementsGamesSteam", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idGame", id);
                        cmd.Parameters.AddWithValue("@Nome", listAchievements[i].nome);
                        cmd.Parameters.AddWithValue("@descricao", listAchievements[i].descricao);
                        if (listAchievements[i].Obtido == "1")
                        {
                            cmd.Parameters.AddWithValue("@Ativo", 1);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Ativo", 0);
                        }
                        cmd.Parameters.AddWithValue("@iconLock", listAchievements[i].IconLock);
                        cmd.Parameters.AddWithValue("@iconUnlock", listAchievements[i].Icon);
                        if (data == "01/01/1970 00:00")
                        {
                            cmd.Parameters.AddWithValue("@dataObtido", default);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@dataObtido", Convert.ToDateTime(data));
                        }
                        listAchievements[1].porcent = 99;
                        cmd.Parameters.AddWithValue("@porcentagem", listAchievements[i].porcent);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void spUpdSttsAchievementsGames(AchievementAll[] listAchievements, int idGame)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                for (int i = 0; i <= listAchievements.Length - 1; i++)
                {
                    using (SqlCommand cmd = new SqlCommand("spUpdSttsAchievementsGames", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@idGame", idGame);
                        cmd.Parameters.AddWithValue("@nome", listAchievements[i].nome);
                        cmd.Parameters.AddWithValue("@porcentagem", listAchievements[i].porcent);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void spInsPorcentagemGamesSteam(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsPorcentagemGamesSteam", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idGame", id);
                    cmd.ExecuteNonQuery();
                }

            }
        }

        public void spInsDadosProfileSteam()
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsDadosProfileSteam", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spInsDestaqueAchSteam(int id, string achievement, int? idDestaque)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsDestaqueAchSteam", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@idGame", id);
                    cmd.Parameters.AddWithValue("@achievement", achievement);
                    cmd.Parameters.AddWithValue("@id", idDestaque);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spInsMetaGame(int idGame, int ano, int mes)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsMetaGame", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idGame", idGame);
                    cmd.Parameters.AddWithValue("@ano", ano);
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spUpdFavScreen(int id, int idScreen)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spUpdFavScreen", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@idScreen", idScreen);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spInsFavoriteGameSteam(int idGame, int plataform)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsFavoriteGameSteam", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idGame", idGame);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@plataform", plataform);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spDelFavoriteGame(int idGame)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spDelFavoriteGame", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idGame", idGame);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spInsScreenshotGame(string nome, string descricao, byte[] imagem, int? id, string pasta)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsScreenshotGame", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@categoria", null);
                    cmd.Parameters.AddWithValue("@descricao", descricao);
                    cmd.Parameters.AddWithValue("@imagens", imagem);
                    cmd.Parameters.AddWithValue("@idGame", id);
                    cmd.Parameters.AddWithValue("@folder", pasta);
                    cmd.Parameters.AddWithValue("@ativo", 1);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spInsItemFolder(string pasta)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsItemFolder", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@folder", pasta);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spInsFranchiseGameSteam(string title, int? idGame, int? tipo, string nomeFranquia)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsFranchiseGameSteam", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@idGame", idGame);
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@nomeFranquia", nomeFranquia);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spInsImgScreenShotItlg(byte[] imagem, long? idGame, string nome, int plataform)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsScreenshotGame", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@categoria", nome);
                    cmd.Parameters.AddWithValue("@descricao", "nula");
                    cmd.Parameters.AddWithValue("@imagens", imagem);
                    cmd.Parameters.AddWithValue("@idGame", idGame);
                    cmd.Parameters.AddWithValue("@ativo", 1);
                    cmd.Parameters.AddWithValue("@plataform", plataform);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spInsfavoritarImgScreenShot(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsfavoritarImgScreenShot", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@dtFav", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spInsMusica(string nome, byte[] file)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsMusica", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@sound", file);
                    cmd.Parameters.AddWithValue("@dataInclusao", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spInsNoteGame(string titulo, string mensagem, int idGame)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spInsNoteGame", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@titulo", titulo);
                    cmd.Parameters.AddWithValue("@mensagem", mensagem);
                    cmd.Parameters.AddWithValue("@idGame", idGame);
                    cmd.Parameters.AddWithValue("@data", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spUpdGamesScreenshot(int id, int tipo, string mensagem, int plataform)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spUpdGamesScreenshot", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@tipo", tipo);

                    if (tipo != 3)
                    {
                        cmd.Parameters.AddWithValue("@mensagem", mensagem);
                    }
                    if (tipo == 3)
                    {
                        cmd.Parameters.AddWithValue("@mensagem", Convert.ToInt32(mensagem));
                        cmd.Parameters.AddWithValue("@data", DateTime.Now);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spUpdEditNoteGame(int id, string mensagem)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spUpdEditNoteGame", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@mensagem", mensagem);
                    cmd.Parameters.AddWithValue("@data", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<JogosDTO> spSelGamesSteam(int? tipo)
        {
            string procedure = "";
            if (tipo == null)
            {
                procedure = "spSelGamesSteam";
            }
            else
            {
                procedure = "spSelGamesFilter";
            }

            List<JogosDTO> result = new List<JogosDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(procedure, connection))
                {
                    if (tipo != null)
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@tipo", tipo);
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.identificador = Convert.ToInt64(reader["id"]);
                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.ativo = Convert.ToInt32(reader["ativo"]);
                            dto.icon = reader["icon"].ToString();
                            dto.imagem = reader["Imagem"].ToString();
                            dto.categoria = reader["categoria"].ToString();
                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imagemCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            if (!reader["banner"].Equals(DBNull.Value))
                                dto.banner = Convert.ToBase64String((byte[])reader["banner"]);
                            if (!reader["time"].Equals(DBNull.Value))
                                dto.timeGame = Convert.ToInt32(reader["time"]);
                            dto.numAllAchievements = Convert.ToInt32(reader["tdsAch"]);
                            dto.numAchievememtUnlock = Convert.ToInt32(reader["obtidos"]);
                            if (!reader["porcent"].Equals(DBNull.Value))
                                dto.porcentagem = Convert.ToInt32(reader["porcent"]);

                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public string spSelNameGame(int idGame, int plataform)
        {
            string result = "";
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelNameGame", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idGame", idGame);
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = reader["nome"].ToString();
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosDTO> spSelAllGames(int plataform)
        {
            List<JogosDTO> result = new List<JogosDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelAllGames", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.identificador = Convert.ToInt64(reader["id"]);
                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.ativo = Convert.ToInt32(reader["ativo"]);
                            dto.icon = reader["icon"].ToString();
                            dto.imagem = reader["Imagem"].ToString();
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosDTO> spSelGamesRetro()
        {
            List<JogosDTO> result = new List<JogosDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelGamesRetro", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.identificador = Convert.ToInt64(reader["id"]);
                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.ativo = Convert.ToInt32(reader["ativo"]);
                            dto.icon = reader["icon"].ToString();
                            dto.imagem = reader["imagem"].ToString();
                            dto.numAllAchievements = Convert.ToInt32(reader["tdsAch"]);
                            dto.numAchievememtUnlock = Convert.ToInt32(reader["obtidos"]);
                            if (!reader["porcentagem"].Equals(DBNull.Value))
                                dto.porcentagem = Convert.ToInt32(reader["porcentagem"]);
                            dto.plataformaG = reader["plataforma"].ToString();

                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosDTO> spSelFranchiseGames()
        {
            List<JogosDTO> result = new List<JogosDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelFranchiseGames", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.identificador = Convert.ToInt64(reader["id"]);
                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["franquia"].ToString();

                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosDTO> spSelGameFranchiseId(string franquia, string plataform)
        {
            List<JogosDTO> result = new List<JogosDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelGameFranchiseId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@franquia", franquia);
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.nomeFranquia = franquia;
                            dto.icon = reader["icon"].ToString();
                            dto.plataforma = Convert.ToInt32(reader["plataform"]);
                            if (dto.plataforma == 0)
                                dto.imagem = reader["Imagem"].ToString();
                            else
                                dto.imagem = reader["Imagem"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");
                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imagemCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            if (!reader["banner"].Equals(DBNull.Value))
                                dto.banner = Convert.ToBase64String((byte[])reader["banner"]);
                            dto.categoria = reader["categoria"].ToString();
                            if (!reader["dtCategoria"].Equals(DBNull.Value))
                                dto.dtObtido = Convert.ToDateTime(reader["dtCategoria"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<PercentGameDTO> spSelRarestAchiev()
        {
            List<PercentGameDTO> result = new List<PercentGameDTO>();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelRarestAchiev", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int cont = 0;
                        while (reader.Read() && cont <= 4)
                        {
                            PercentGameDTO dto = new PercentGameDTO();

                            if (!reader["idGame"].Equals(DBNull.Value))
                                dto.idGame = Convert.ToInt32(reader["idGame"]);
                            dto.nomeGame = reader["nomeGame"].ToString();
                            dto.nome = reader["name"].ToString();
                            dto.descricao = reader["descricao"].ToString();
                            dto.icon = reader["icon"].ToString();
                            dto.porcentagem = Convert.ToDouble(reader["porcent"]);
                            dto.plataform = Convert.ToInt32(reader["plataform"]);
                            result.Add(dto);
                            cont++;
                        }
                    }
                }
            }
            return result;
        }

        public List<FolderGamesDTO> spSelAllFolders()
        {
            List<FolderGamesDTO> result = new List<FolderGamesDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelAllFolders", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FolderGamesDTO dto = new FolderGamesDTO();

                            dto.id = Convert.ToInt32(reader["id"]);
                            dto.nome = reader["nome"].ToString();
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosListDTO> spSelAllGamesList(int? tipo, int? plataform, int layout)
        {
            string procedure = "";
            if (tipo == null)
            {
                procedure = "spSelAllGamesList";
            }
            else
            {
                procedure = "spSelGamesListGameFilter";
            }
            List<JogosListDTO> result = new List<JogosListDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(procedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (tipo != null)
                    {
                        command.Parameters.AddWithValue("@tipo", tipo);
                    }
                    command.Parameters.AddWithValue("@plataform", plataform);
                    command.Parameters.AddWithValue("@layout", layout);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosListDTO dto = new JogosListDTO();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.Intplataforma = Convert.ToInt32(reader["plataform"]);
                            if (dto.Intplataforma == 0)
                                dto.imagem = reader["Imagem"].ToString(); dto.imagem = "https://cdn.cloudflare.steamstatic.com/steam/apps/" + dto.id + "/header.jpg";
                            if (dto.Intplataforma == 1)
                                dto.imagem = reader["Imagem"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");
                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imgCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            dto.ativo = Convert.ToInt32(reader["ativo"]);
                            if (!reader["favoritos"].Equals(DBNull.Value))
                                dto.favoritos = Convert.ToInt32(reader["favoritos"]);
                            if (!reader["time"].Equals(DBNull.Value))
                                dto.timeGame = Convert.ToInt32(reader["time"]);
                            dto.numAllAchievements = Convert.ToInt32(reader["tdsAch"]);
                            dto.numAchievememtUnlock = Convert.ToInt32(reader["obtidos"]);
                            if (!reader["dtLastAchiev"].Equals(DBNull.Value))
                                dto.dateLastAchievement = Convert.ToDateTime(reader["dtLastAchiev"]);
                            if (!reader["porcentagem"].Equals(DBNull.Value))
                                dto.porcentagem = Convert.ToInt32(reader["porcentagem"]);
                            dto.plataforma = reader["plataforma"].ToString();
                            dto.Indexplataforma = Convert.ToInt32(plataform);
                            if (layout != 0)
                            {
                                dto.ordenagem = Convert.ToInt32(reader["ordenagem"]);
                            }
                            dto.categoria = reader["categoria"].ToString();
                            if (dto.Intplataforma == 0)
                                dto.iconAchievements = reader["iconAchiev"].ToString();
                            else
                                dto.iconAchievements = reader["iconAchiev"].ToString() + ".png";
                            dto.nomeAchiev = reader["nomeAchiev"].ToString();
                            if (!reader["banner"].Equals(DBNull.Value))
                                dto.banner = Convert.ToBase64String((byte[])reader["banner"]);
                            if (!reader["imgCustomGrid"].Equals(DBNull.Value))
                                dto.imgCustomGrid = Convert.ToBase64String((byte[])reader["imgCustomGrid"]);
                            if (!reader["imgGrid"].Equals(DBNull.Value))
                                if (dto.Intplataforma == 0)
                                    dto.imgGrid = reader["imgGrid"].ToString();
                            if (dto.Intplataforma == 1)
                                dto.imgGrid = reader["imgGrid"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosListDTO> spSelGamesListGame(int? tipo)
        {
            string procedure = "";
            if (tipo == null)
            {
                procedure = "spSelGamesListGame";
            }
            else
            {
                procedure = "spSelGamesListGameFilter";
            }

            List<JogosListDTO> result = new List<JogosListDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(procedure, connection))
                {
                    if (tipo != null)
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@tipo", tipo);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosListDTO dto = new JogosListDTO();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.ativo = Convert.ToInt32(reader["ativo"]);
                            if (!reader["favoritos"].Equals(DBNull.Value))
                                dto.favoritos = Convert.ToInt32(reader["favoritos"]);
                            dto.timeGame = Convert.ToInt32(reader["time"]);
                            dto.numAllAchievements = Convert.ToInt32(reader["tdsAch"]);
                            dto.numAchievememtUnlock = Convert.ToInt32(reader["obtidos"]);
                            if (!reader["date"].Equals(DBNull.Value))
                                dto.dateLastAchievement = Convert.ToDateTime(reader["date"]);
                            if (!reader["porcent"].Equals(DBNull.Value))
                                dto.porcentagem = Convert.ToInt32(reader["porcent"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosListDTO> spSelListGamesRetro()
        {
            List<JogosListDTO> result = new List<JogosListDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelListGamesRetro", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosListDTO dto = new JogosListDTO();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.imagem = reader["imagem"].ToString();
                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imgCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            dto.ativo = Convert.ToInt32(reader["ativo"]);
                            dto.numAllAchievements = Convert.ToInt32(reader["tdsAch"]);
                            dto.numAchievememtUnlock = Convert.ToInt32(reader["obtidos"]);
                            if (!reader["date"].Equals(DBNull.Value))
                                dto.dateLastAchievement = Convert.ToDateTime(reader["date"]);
                            if (!reader["porcent"].Equals(DBNull.Value))
                                dto.porcentagem = Convert.ToInt32(reader["porcent"]);
                            dto.plataforma = reader["plataforma"].ToString();
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public JogosDTO spSelGameRetroPorId(int idGame)
        {
            JogosDTO result = new JogosDTO();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelGameRetroPorId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idGame", idGame);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.plataforma = Convert.ToInt32(reader["consoleId"]);
                            dto.developer = reader["developer"].ToString();
                            dto.publisher = reader["publiser"].ToString();
                            dto.dtLancamento = Convert.ToDateTime(reader["dateReleased"]);
                            dto.icon = reader["icon"].ToString();
                            dto.imagem = reader["imagem"].ToString();
                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imagemCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            dto.ativo = Convert.ToInt32(reader["ativo"]);
                            if (!reader["porcentagem"].Equals(DBNull.Value))
                                dto.porcentagem = Convert.ToInt32(reader["porcentagem"]);

                            result = dto;
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosListDTO> spSelGamesListGameCateg(string categ)
        {
            List<JogosListDTO> result = new List<JogosListDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelGamesListGameCateg", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@categ", categ);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosListDTO dto = new JogosListDTO();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.ativo = Convert.ToInt32(reader["ativo"]);
                            dto.timeGame = Convert.ToInt32(reader["time"]);
                            dto.numAllAchievements = Convert.ToInt32(reader["tdsAch"]);
                            dto.numAchievememtUnlock = Convert.ToInt32(reader["obtidos"]);
                            if (!reader["date"].Equals(DBNull.Value))
                                dto.dateLastAchievement = Convert.ToDateTime(reader["date"]);
                            dto.porcentagem = Convert.ToInt32(reader["porcent"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosListDTO> spSelSearchListGame(string search, int? plataform)
        {
            List<JogosListDTO> result = new List<JogosListDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelSearchListGame", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@search", search);
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosListDTO dto = new JogosListDTO();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.Intplataforma = Convert.ToInt32(reader["plataform"]);
                            if (dto.Intplataforma == 0)
                                dto.imagem = reader["Imagem"].ToString(); dto.imagem = "https://cdn.cloudflare.steamstatic.com/steam/apps/" + dto.id + "/header.jpg";
                            if (dto.Intplataforma == 1)
                                dto.imagem = reader["Imagem"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");
                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imgCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            dto.ativo = Convert.ToInt32(reader["ativo"]);
                            if (!reader["favoritos"].Equals(DBNull.Value))
                                dto.favoritos = Convert.ToInt32(reader["favoritos"]);
                            if (!reader["time"].Equals(DBNull.Value))
                                dto.timeGame = Convert.ToInt32(reader["time"]);
                            dto.numAllAchievements = Convert.ToInt32(reader["tdsAch"]);
                            dto.numAchievememtUnlock = Convert.ToInt32(reader["obtidos"]);
                            if (!reader["dtLastAchiev"].Equals(DBNull.Value))
                                dto.dateLastAchievement = Convert.ToDateTime(reader["dtLastAchiev"]);
                            if (!reader["porcentagem"].Equals(DBNull.Value))
                                dto.porcentagem = Convert.ToInt32(reader["porcentagem"]);
                            dto.plataforma = reader["plataforma"].ToString();
                            dto.Indexplataforma = Convert.ToInt32(plataform);
                            //if (layout != 0)
                            //{
                            //    dto.ordenagem = Convert.ToInt32(reader["ordenagem"]);
                            //}
                            dto.categoria = reader["categoria"].ToString();
                            if (dto.Intplataforma == 0)
                                dto.iconAchievements = reader["iconAchiev"].ToString();
                            else
                                dto.iconAchievements = reader["iconAchiev"].ToString() + ".png";
                            dto.nomeAchiev = reader["nomeAchiev"].ToString();
                            if (!reader["banner"].Equals(DBNull.Value))
                                dto.banner = Convert.ToBase64String((byte[])reader["banner"]);
                            if (!reader["imgCustomGrid"].Equals(DBNull.Value))
                                dto.imgCustomGrid = Convert.ToBase64String((byte[])reader["imgCustomGrid"]);
                            if (!reader["imgGrid"].Equals(DBNull.Value))
                                if (dto.Intplataforma == 0)
                                    dto.imgGrid = reader["imgGrid"].ToString();
                            if (dto.Intplataforma == 1)
                                dto.imgGrid = reader["imgGrid"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosListDTO> spSelCategListGame(string categ, int? plataform)
        {
            List<JogosListDTO> result = new List<JogosListDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spsSelCategListGame", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@categ", categ);
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosListDTO dto = new JogosListDTO();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.Intplataforma = Convert.ToInt32(reader["plataform"]);
                            if (dto.Intplataforma == 0)
                                dto.imagem = reader["Imagem"].ToString(); dto.imagem = "https://cdn.cloudflare.steamstatic.com/steam/apps/" + dto.id + "/header.jpg";
                            if (dto.Intplataforma == 1)
                                dto.imagem = reader["Imagem"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");
                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imgCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            dto.ativo = Convert.ToInt32(reader["ativo"]);
                            if (!reader["favoritos"].Equals(DBNull.Value))
                                dto.favoritos = Convert.ToInt32(reader["favoritos"]);
                            if (!reader["time"].Equals(DBNull.Value))
                                dto.timeGame = Convert.ToInt32(reader["time"]);
                            dto.numAllAchievements = Convert.ToInt32(reader["tdsAch"]);
                            dto.numAchievememtUnlock = Convert.ToInt32(reader["obtidos"]);
                            if (!reader["dtLastAchiev"].Equals(DBNull.Value))
                                dto.dateLastAchievement = Convert.ToDateTime(reader["dtLastAchiev"]);
                            if (!reader["porcentagem"].Equals(DBNull.Value))
                                dto.porcentagem = Convert.ToInt32(reader["porcentagem"]);
                            dto.plataforma = reader["plataforma"].ToString();
                            dto.Indexplataforma = Convert.ToInt32(plataform);
                            //if (layout != 0)
                            //{
                            //    dto.ordenagem = Convert.ToInt32(reader["ordenagem"]);
                            //}
                            dto.categoria = reader["categoria"].ToString();
                            if (dto.Intplataforma == 0)
                                dto.iconAchievements = reader["iconAchiev"].ToString();
                            else
                                dto.iconAchievements = reader["iconAchiev"].ToString() + ".png";
                            dto.nomeAchiev = reader["nomeAchiev"].ToString();
                            if (!reader["banner"].Equals(DBNull.Value))
                                dto.banner = Convert.ToBase64String((byte[])reader["banner"]);
                            if (!reader["imgCustomGrid"].Equals(DBNull.Value))
                                dto.imgCustomGrid = Convert.ToBase64String((byte[])reader["imgCustomGrid"]);
                            if (!reader["imgGrid"].Equals(DBNull.Value))
                                if (dto.Intplataforma == 0)
                                    dto.imgGrid = reader["imgGrid"].ToString();
                            if (dto.Intplataforma == 1)
                                dto.imgGrid = reader["imgGrid"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosListDTO> spSelForAchievements(int id, int? plataform)
        {
            List<JogosListDTO> result = new List<JogosListDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelForAchievements", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idGame", id);
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosListDTO dto = new JogosListDTO();

                            dto.Intplataforma = Convert.ToInt32(reader["plataform"]);
                            if (dto.Intplataforma == 0)
                                dto.ForAchievements = reader["iconUnlock"].ToString();
                            else
                                dto.ForAchievements = "https://retroachievements.org/Badge/" + reader["iconUnlock"].ToString() + ".png";
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<GamesLstDesejo> spselGamesListDesejo()
        {
            List<GamesLstDesejo> result = new List<GamesLstDesejo>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("select * from tbListaDesejosGamesSteam", connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GamesLstDesejo dto = new GamesLstDesejo();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.Name = reader["nome"].ToString();
                            dto.image = reader["imagem"].ToString();
                            dto.Review = reader["Review"].ToString();
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public StaticsGameList spSelStaticsGames(int? plataform)
        {
            StaticsGameList result = new StaticsGameList();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelStaticsGames", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StaticsGameList dto = new StaticsGameList();

                            dto.allGames = Convert.ToInt32(reader["allGames"]);
                            if (!reader["porcentagem"].Equals(DBNull.Value))
                                dto.porcentagem = Convert.ToInt32(reader["porcentagem"]);
                            if (!reader["timeGame"].Equals(DBNull.Value))
                                dto.timeGame = Convert.ToInt32(reader["timeGame"]);
                            dto.allAchievements = Convert.ToInt32(reader["allAchievements"]);
                            dto.allPlatinas = Convert.ToInt32(reader["allPlatinas"]);
                            result = dto;
                        }
                    }
                }
            }
            return result;
        }

        public void spUpdStaticsProfileGames(int porcentagem, int time, int award, int games, int platinas)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spUpdStaticsProfileGames", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@porcentagem", porcentagem);
                    cmd.Parameters.AddWithValue("@time", time);
                    cmd.Parameters.AddWithValue("@award", award);
                    cmd.Parameters.AddWithValue("@games", games);
                    cmd.Parameters.AddWithValue("@platinas", platinas);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<JogosDTO> spselSlideGames(int? plataform)
        {
            List<JogosDTO> result = new List<JogosDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelBannerGames", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@plataform", plataform);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.banner = Convert.ToBase64String((byte[])reader["banner"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosDTO> spselSlideGamesPorId(int? plataform, string identificador, int tipo)
        {
            List<JogosDTO> result = new List<JogosDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelBannerGamesPorId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@plataform", plataform);
                    command.Parameters.AddWithValue("@identificador", identificador);
                    command.Parameters.AddWithValue("@tipo", tipo);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.banner = Convert.ToBase64String((byte[])reader["banner"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public JogosDTO spSelGamesPorId(int id, int plataforma)
        {
            JogosDTO result = new JogosDTO();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelGamesPorId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@plataform", plataforma);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            if (plataforma == 0)
                                dto.imagem = reader["Imagem"].ToString();
                            else
                                dto.imagem = reader["Imagem"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");

                            dto.categoria = reader["categoria"].ToString();
                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imagemCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            if (!reader["imgHeader"].Equals(DBNull.Value))
                                dto.imgHeader = Convert.ToBase64String((byte[])reader["imgHeader"]);
                            if (!reader["banner"].Equals(DBNull.Value))
                                dto.banner = Convert.ToBase64String((byte[])reader["banner"]);
                            dto.porcentagem = Convert.ToInt32(reader["porcentagem"]);
                            if (!reader["time"].Equals(DBNull.Value))
                                dto.timeGame = Convert.ToInt32(reader["time"]);
                            dto.dtObtido = Convert.ToDateTime(reader["dateUnlock"]);
                            if (plataforma == 0)
                                dto.icon = reader["iconUnlock"].ToString();
                            else
                                dto.icon = reader["iconUnlock"].ToString() + ".png";
                            dto.numAllAchievements = Convert.ToInt32(reader["tdsAch"]);
                            dto.numAchievememtUnlock = Convert.ToInt32(reader["obtidos"]);
                            dto.developer = reader["developer"].ToString();
                            dto.publisher = reader["publisher"].ToString();
                            dto.plataformaG = reader["namePlataform"].ToString();
                            dto.plataforma = plataforma;
                            result = dto;
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosListDTO> spSelGamesSteamPorCateg(string categoria, string plataform)
        {
            List<JogosListDTO> result = new List<JogosListDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelGamesSteamPorCateg", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@categoria", categoria);
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosListDTO dto = new JogosListDTO();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            //dto.icon = reader["icon"].ToString();
                            dto.porcentagem = Convert.ToInt32(reader["porcentagem"]);
                            dto.Intplataforma = Convert.ToInt32(reader["plataform"]);
                            if (dto.Intplataforma == 0)
                                dto.imagem = reader["Imagem"].ToString();
                            else
                                dto.imagem = reader["Imagem"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");
                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imgCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            if (!reader["banner"].Equals(DBNull.Value))
                                dto.banner = Convert.ToBase64String((byte[])reader["banner"]);
                            dto.categoria = reader["categoria"].ToString();
                            if (!reader["dtCategoria"].Equals(DBNull.Value))
                                dto.dateLastAchievement = Convert.ToDateTime(reader["dtCategoria"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosDTO> spSelGamesScreenPorFolder(int folder)
        {
            List<JogosDTO> result = new List<JogosDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelGamesScreenPorFolder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@folder", folder);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            if (!reader["idGame"].Equals(DBNull.Value))
                                dto.id = Convert.ToInt32(reader["idGame"]);
                            dto.icon = reader["icon"].ToString();
                            dto.nome = reader["nome"].ToString();
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        //public List<CategGamesDTO> spSelListGamesCateg()
        //{
        //    List<CategGamesDTO> result = new List<CategGamesDTO>();

        //    using (SqlConnection connection = new SqlConnection(connectString))
        //    {
        //        connection.Open();
        //        using (SqlCommand command = new SqlCommand("spSelListGamesCateg", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;

        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    CategGamesDTO dto = new CategGamesDTO();

        //                    dto.nome = reader["categoria"].ToString();
        //                    result.Add(dto);
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}

        public List<JogosDTO> spselCategoriasGames()
        {
            List<JogosDTO> result = new List<JogosDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spselCategoriasGames", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.categoria = reader["categoria"].ToString();
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public int spSelLastGameCateg(string categ)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelLastGameCateg", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@categ", categ);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = Convert.ToInt32(reader["idGame"].ToString());
                        }
                    }
                }
            }
            return result;
        }

        public List<FolderGamesDTO> spSelFoldersGames()
        {
            List<FolderGamesDTO> result = new List<FolderGamesDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelFoldersGames", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FolderGamesDTO dto = new FolderGamesDTO();

                            dto.id = Convert.ToInt32(reader["id"]);
                            dto.nome = reader["nome"].ToString();
                            dto.imagens = Convert.ToBase64String((byte[])reader["imagens"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosDTO> spSelGamesSteamForSearch(string search)
        {
            List<JogosDTO> result = new List<JogosDTO>();

            string procedure = "";
            if (search != "" && search != null)
            {
                procedure = "spSelGamesSteamForSearch";
            }
            else
            {
                procedure = "spSelGamesListIcons";
            }
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(procedure, connection))
                {
                    if (search != "" && search != null)
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@search", search);
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.plataforma = Convert.ToInt32(reader["plataform"]);
                            dto.nome = reader["nome"].ToString();
                            if (dto.plataforma == 0)
                                dto.icon = reader["icon"].ToString();
                            else
                                dto.icon = reader["icon"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosDTO> spSelAllGamesScreen()
        {
            List<JogosDTO> result = new List<JogosDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelAllGamesScreen", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["categoria"].ToString();
                            dto.icon = reader["icon"].ToString();
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosDTO> spSelGamesSteamForPercentual(int porcent)
        {
            List<JogosDTO> result = new List<JogosDTO>();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelGamesSteamForPercentual", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@porcent", porcent);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.icon = reader["icon"].ToString();
                            dto.imagem = reader["Imagem"].ToString();
                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imagemCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<AchievementsDTO> spSelDestaqueAchSteam()
        {
            List<AchievementsDTO> result = new List<AchievementsDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelDestaqueAchSteam", connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AchievementsDTO dto = new AchievementsDTO();

                            dto.id = Convert.ToInt64(reader["id"]);
                            dto.iconUnlock = reader["nome"].ToString();
                            dto.dateDestaque = Convert.ToDateTime(reader["date"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<ScreenshotDTO> spSelFavScreenshoot()
        {
            List<ScreenshotDTO> result = new List<ScreenshotDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelFavScreenshoot", connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ScreenshotDTO dto = new ScreenshotDTO();

                            dto.id = Convert.ToInt64(reader["id"]);
                            dto.nome = reader["nome"].ToString();
                            dto.imagens = Convert.ToBase64String((byte[])reader["imagens"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<ScreenshotDTO> spSelAllFavScreenshoot()
        {
            List<ScreenshotDTO> result = new List<ScreenshotDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelAllFavScreenshoot", connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ScreenshotDTO dto = new ScreenshotDTO();

                            dto.id = Convert.ToInt64(reader["id"]);
                            dto.nome = reader["nome"].ToString();
                            dto.imagens = Convert.ToBase64String((byte[])reader["imagens"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosDTO> spSelMetaGames(int ano, int mes)
        {
            List<JogosDTO> result = new List<JogosDTO>();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelMetaGames", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ano", ano);
                    command.Parameters.AddWithValue("@mes", mes);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.id = Convert.ToInt32(reader["idGame"]);
                            dto.plataforma = Convert.ToInt32(reader["plataform"]);
                            dto.nome = reader["nome"].ToString();
                            if (dto.plataforma == 0)
                                dto.imagem = reader["Imagem"].ToString();
                            else
                                dto.imagem = reader["Imagem"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");

                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imagemCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            dto.porcentagem = Convert.ToInt32(reader["porcentagem"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        } 

        public List<FeedGames> spSelHistoricoGames(int? tipo)
        {
            List<FeedGames> result = new List<FeedGames>();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelHistoricoGames", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@tipo", tipo);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int cont = 0;
                        while (reader.Read())
                        {
                            FeedGames dto = new FeedGames();

                            dto.id = Convert.ToInt32(reader["id"]);
                            dto.idGame = Convert.ToInt32(reader["idGame"]);
                            if (dto.idGame != 20920)
                            {
                                dto.dateObj = Convert.ToDateTime(reader["dateObj"]);
                            }
                            dto.tipo = Convert.ToInt32(reader["tipo"]);
                            dto.plataform = Convert.ToInt32(reader["plataform"]);
                            dto.namePlataform = reader["namePlataform"].ToString();
                            dto.nome = reader["nome"].ToString();
                            if (dto.plataform == 0)
                                dto.Imagem = reader["Imagem"].ToString();
                            else
                                dto.Imagem = reader["Imagem"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");

                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imgCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            if (!reader["time"].Equals(DBNull.Value))
                                dto.time = Convert.ToInt32(reader["time"]);
                            dto.porcentagem = Convert.ToInt32(reader["porcentagem"]);
                            if (dto.plataform == 0)
                                dto.icon = reader["icon"].ToString();
                            else
                                dto.icon = reader["icon"].ToString() + ".png";
                            dto.nomeAch = reader["nomeAch"].ToString();
                            //dto.titulo = reader["titulo"].ToString();
                            //dto.mensagem = reader["mensagem"].ToString();
                            dto.nomeProfile = reader["nomeProf"].ToString();
                            if (dto.plataform == 0)
                                dto.avatar = "https://avatars.akamai.steamstatic.com/c3e0d0878340537330f90971ea23e11a42d1143f_full.jpg";
                            else
                                dto.avatar = reader["avatar"].ToString().Replace("/UserPic/", "https://media.retroachievements.org/UserPic/");
                            result.Add(dto);
                            cont++;
                        }
                    }
                }
            }
            return result;
        }

        public FeedGames spSelLastCompletism()
        {
            FeedGames result = new FeedGames();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelLastCompletism", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            result.id = Convert.ToInt32(reader["id"]);
                            result.idGame = Convert.ToInt32(reader["idGame"]);
                            result.dateObj = Convert.ToDateTime(reader["dateObj"]);
                            result.tipo = Convert.ToInt32(reader["tipo"]);
                            result.plataform = Convert.ToInt32(reader["plataform"]);
                            result.namePlataform = reader["namePlataform"].ToString();
                            result.nome = reader["nome"].ToString();
                            if (result.plataform == 0)
                                result.Imagem = reader["Imagem"].ToString();
                            else
                                result.Imagem = reader["Imagem"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");

                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                result.imgCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            if (!reader["time"].Equals(DBNull.Value))
                                result.time = Convert.ToInt32(reader["time"]);
                            result.porcentagem = Convert.ToInt32(reader["porcentagem"]);
                            if (result.plataform == 0)
                                result.icon = reader["icon"].ToString();
                            else
                                result.icon = reader["icon"].ToString() + ".png";
                            result.nomeAch = reader["nomeAch"].ToString();
                            result.titulo = reader["titulo"].ToString();
                            result.mensagem = reader["mensagem"].ToString();
                            result.nomeProfile = reader["nomeProf"].ToString();
                            if (result.plataform == 0)
                                result.avatar = "https://avatars.akamai.steamstatic.com/c3e0d0878340537330f90971ea23e11a42d1143f_full.jpg";
                            else
                                result.avatar = reader["avatar"].ToString().Replace("/UserPic/", "https://media.retroachievements.org/UserPic/");
                        }
                    }
                }
            }
            return result;
        }

        public GamesEstatisticas spSeldataProfileStatistic()
        {
            GamesEstatisticas result = new GamesEstatisticas();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSeldataProfileStatistic", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@plataform", 0);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            GamesEstatisticas dto = new GamesEstatisticas();

                            dto.plataformInt = Convert.ToInt32(reader["plataformInt"]);
                            dto.plataformName = reader["plataformName"].ToString();
                            dto.nameProfile = reader["nome"].ToString();
                            dto.imgProfile = "https://avatars.akamai.steamstatic.com/c3e0d0878340537330f90971ea23e11a42d1143f_full.jpg";

                            result = dto;

                        }
                    }
                }
            }
            return result;
        }

        public List<LastPlay> spSelDataStaticsGames()
        {
            List<LastPlay> result = new List<LastPlay>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelDataStaticsGames", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@plataform", 0);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LastPlay listLp = new LastPlay();

                            listLp.idGame = Convert.ToInt32(reader["idGame"]);
                            listLp.nome = reader["nome"].ToString();
                            listLp.imagem = reader["Imagem"].ToString();
                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                listLp.imgCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            listLp.progress = Convert.ToInt32(reader["porcentagem"]);
                            listLp.imgAchiev = reader["imgAchiev"].ToString();
                            listLp.nameAchiev = reader["nameAchiev"].ToString();

                            result.Add(listLp);
                        }
                    }
                }
            }

            return result;
        }

        public List<IndiceGames> spSelIndiceGames(int? year)
        {
            List<IndiceGames> result = new List<IndiceGames>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelIndiceGames", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@year", DateTime.Now.Year);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IndiceGames listInd = new IndiceGames();

                            listInd.year = Convert.ToInt32(reader["ano"]);
                            listInd.month = Convert.ToInt32(reader["mes"]);
                            listInd.nmMonth = reader["nomeMes"].ToString();
                            listInd.num = Convert.ToInt32(reader["numero"]);

                            result.Add(listInd);
                        }
                    }
                }
            }

            return result;
        }

        public List<FeedGames> spSelFeedGames()
        {
            List<FeedGames> result = new List<FeedGames>();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelFeedGames", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int cont = 0;

                        //while (reader.Read() && cont <= 6)
                        while (reader.Read())
                        {
                            FeedGames dto = new FeedGames();

                            dto.id = Convert.ToInt32(reader["id"]);
                            dto.idGame = Convert.ToInt32(reader["idGame"]);
                            dto.dateObj = Convert.ToDateTime(reader["dateObj"]);
                            dto.tipo = Convert.ToInt32(reader["tipo"]);
                            dto.plataform = Convert.ToInt32(reader["plataform"]);
                            dto.namePlataform = reader["namePlataform"].ToString();
                            dto.nome = reader["nome"].ToString();
                            if (dto.plataform == 0)
                                dto.Imagem = reader["Imagem"].ToString();
                            else
                                dto.Imagem = reader["Imagem"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");

                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imgCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            if (!reader["time"].Equals(DBNull.Value))
                                dto.time = Convert.ToInt32(reader["time"]);
                            dto.porcentagem = Convert.ToInt32(reader["porcentagem"]);
                            if (dto.plataform == 0)
                                dto.icon = reader["icon"].ToString();
                            else
                                dto.icon = reader["icon"].ToString() + ".png";
                            dto.nomeAch = reader["nomeAch"].ToString();
                            dto.titulo = reader["titulo"].ToString();
                            dto.mensagem = reader["mensagem"].ToString();
                            dto.nomeProfile = reader["nomeProf"].ToString();
                            if (dto.plataform == 0)
                                dto.avatar = "https://avatars.akamai.steamstatic.com/c3e0d0878340537330f90971ea23e11a42d1143f_full.jpg";
                            else
                                dto.avatar = reader["avatar"].ToString().Replace("/UserPic/", "https://media.retroachievements.org/UserPic/");
                            result.Add(dto);
                            cont++;
                        }
                    }
                }
            }
            return result;
        }

        public List<FeedGames> spSelLastNotes()
        {
            List<FeedGames> result = new List<FeedGames>();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelLastNotes", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FeedGames dto = new FeedGames();

                            dto.idGame = Convert.ToInt32(reader["idGame"]);
                            dto.dateObj = Convert.ToDateTime(reader["data"]);
                            dto.plataform = Convert.ToInt32(reader["plataform"]);
                            dto.nome = reader["nome"].ToString();
                            if (dto.plataform == 0)
                                dto.Imagem = reader["Imagem"].ToString();
                            else
                                dto.Imagem = reader["Imagem"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");

                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imgCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);                           
                            if (dto.plataform == 0)
                                dto.icon = reader["icon"].ToString();
                            else
                                dto.icon = reader["icon"].ToString() + ".png";
                            dto.titulo = reader["titulo"].ToString();
                            dto.mensagem = reader["mensagem"].ToString();
                            
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosDTO> spSelFavGamesSteam(int pagina)
        {
            List<JogosDTO> result = new List<JogosDTO>();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelFavGamesSteam", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@page", pagina);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.id = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.icon = reader["icon"].ToString();
                            dto.imagem = reader["Imagem"].ToString();
                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imagemCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public int spSelCountSizeFavGame()
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelCountSizeFavGame", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = Convert.ToInt32(reader["countSize"]);
                        }
                    }
                }
            }
            return result;
        }

        public DadosGamesSteam spSelDadosGamesInAchievements(int id, int plataform)
        {
            DadosGamesSteam result = new DadosGamesSteam();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelDadosGamesInAchievements", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idGame", id);
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            DadosGamesSteam dto = new DadosGamesSteam();

                            dto.Nome = reader["nome"].ToString();
                            dto.Time = Convert.ToInt32(reader["time"]);
                            if (!reader["dateUnlock"].Equals(DBNull.Value))
                                dto.DataPlatina = Convert.ToDateTime(reader["dateUnlock"]);
                            dto.LastAchiev = reader["iconUnlock"].ToString();
                            result = dto;
                        }
                    }
                }
            }
            return result;
        }

        public List<JogosDTO> spSelCategGamesSteam()
        {
            List<JogosDTO> result = new List<JogosDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelCategGamesSteam", connection))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JogosDTO dto = new JogosDTO();

                            dto.identificador = Convert.ToInt64(reader["id"]);
                            dto.id = Convert.ToInt32(reader["idGame"]);
                            dto.plataforma = Convert.ToInt32(reader["plataform"]);
                            dto.nomeFranquia = reader["categoria"].ToString();
                            if (dto.plataforma == 1)
                                dto.imagem = reader["imagem"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");
                            else
                                dto.imagem = reader["imagem"].ToString();
                            if (!reader["imgCustom"].Equals(DBNull.Value))
                                dto.imagemCustom = Convert.ToBase64String((byte[])reader["imgCustom"]);

                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<ScreenshotDTO> spSelScreenshootGames(string plataform)
        {
            List<ScreenshotDTO> result = new List<ScreenshotDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelScreenshootGames", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ScreenshotDTO dto = new ScreenshotDTO();
                            dto.id = Convert.ToInt64(reader["id"]);
                            if (!reader["idGame"].Equals(DBNull.Value))
                                dto.idGame = Convert.ToInt32(reader["idGame"]);
                            dto.imagens = Convert.ToBase64String((byte[])reader["imagens"]);
                            dto.nome = reader["nome"].ToString();
                            dto.descricao = reader["descricao"].ToString();
                            if (!reader["favorito"].Equals(DBNull.Value))
                                dto.favorito = Convert.ToInt32(reader["favorito"]);
                            dto.plataform = Convert.ToInt32(reader["plataform"]);
                            if (!reader["nomeGame"].Equals(DBNull.Value))
                                dto.nomeGame = reader["nomeGame"].ToString();
                            dto.categoriaGame = reader["categoriaGame"].ToString();
                            if (dto.plataform == 0)
                                dto.icon = reader["iconGame"].ToString();
                            else
                                dto.icon = reader["iconGame"].ToString().Replace("/Images/", "https://media.retroachievements.org/Images/");
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<ScreenshotDTO> spSelScreenshootGamesPorId(int idGame)
        {
            List<ScreenshotDTO> result = new List<ScreenshotDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelScreenshootGamesPorId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idGame", idGame);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ScreenshotDTO dto = new ScreenshotDTO();
                            dto.id = Convert.ToInt64(reader["id"]);
                            if (!reader["idGame"].Equals(DBNull.Value))
                                dto.idGame = Convert.ToInt32(reader["idGame"]);
                            dto.imagens = Convert.ToBase64String((byte[])reader["imagens"]);
                            dto.nome = reader["nome"].ToString();
                            dto.nomeGame = reader["nameGame"].ToString();
                            dto.descricao = reader["descricao"].ToString();
                            if (!reader["favorito"].Equals(DBNull.Value))
                                dto.favorito = Convert.ToInt32(reader["favorito"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<ScreenshotDTO> spSelScreenshootGamesPorFolder(int idFolder)
        {
            List<ScreenshotDTO> result = new List<ScreenshotDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelScreenshootGamesPorFolder", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", idFolder);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ScreenshotDTO dto = new ScreenshotDTO();
                            dto.id = Convert.ToInt64(reader["id"]);
                            if (!reader["idGame"].Equals(DBNull.Value))
                                dto.idGame = Convert.ToInt32(reader["idGame"]);
                            dto.imagens = Convert.ToBase64String((byte[])reader["imagens"]);
                            dto.nome = reader["nome"].ToString();
                            dto.nomeGame = reader["nameGame"].ToString();
                            dto.descricao = reader["descricao"].ToString();
                            if (!reader["favorito"].Equals(DBNull.Value))
                                dto.favorito = Convert.ToInt32(reader["favorito"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<AnotacoesDTO> spSelNotesGamesId(int idGame)
        {
            List<AnotacoesDTO> result = new List<AnotacoesDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelNotesGamesId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idGame", idGame);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AnotacoesDTO dto = new AnotacoesDTO();
                            if (!reader["id"].Equals(DBNull.Value))
                                dto.id = Convert.ToInt32(reader["id"]);
                            if (!reader["idGame"].Equals(DBNull.Value))
                                dto.idGame = Convert.ToInt32(reader["idGame"]);
                            if (!reader["nome"].Equals(DBNull.Value))
                                dto.nome = reader["nome"].ToString();
                            if (!reader["titulo"].Equals(DBNull.Value))
                                dto.titulo = reader["titulo"].ToString();
                            if (!reader["mensagem"].Equals(DBNull.Value))
                                dto.mensagem = reader["mensagem"].ToString();
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public AnotacoesDTO spSelNoteGame()
        {
            AnotacoesDTO result = new AnotacoesDTO();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelNoteGame", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            AnotacoesDTO dto = new AnotacoesDTO();
                            if (!reader["id"].Equals(DBNull.Value))
                                dto.id = Convert.ToInt32(reader["id"]);
                            if (!reader["idGame"].Equals(DBNull.Value))
                                dto.idGame = Convert.ToInt32(reader["idGame"]);
                            if (!reader["nome"].Equals(DBNull.Value))
                                dto.nome = reader["nome"].ToString();
                            if (!reader["titulo"].Equals(DBNull.Value))
                                dto.titulo = reader["titulo"].ToString();
                            if (!reader["mensagem"].Equals(DBNull.Value))
                                dto.mensagem = reader["mensagem"].ToString();
                            result = dto;
                        }
                    }
                }
            }
            return result;
        }

        public void spDelRemoveScreenshot(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spDelRemoveScreenshot", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int spSelPlataformGamePorId(int id, int tipo)
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelPlataformGamePorId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@tipo", tipo);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = Convert.ToInt32(reader["plataform"]);
                        }
                    }
                }
            }
            return result;
        }

        public ScreenshotDTO spSelScreenshootPorId(int id, int plataform)
        {
            ScreenshotDTO result = new ScreenshotDTO();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelScreenshootPorId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ScreenshotDTO dto = new ScreenshotDTO();

                            dto.id = Convert.ToInt64(reader["id"]);
                            if (!reader["idGame"].Equals(DBNull.Value))
                                dto.idGame = Convert.ToInt32(reader["idGame"]);
                            dto.imagens = Convert.ToBase64String((byte[])reader["imagens"]);
                            if (!reader["icon"].Equals(DBNull.Value))
                                dto.icon = reader["icon"].ToString();
                            dto.nome = reader["nome"].ToString();
                            dto.descricao = reader["descricao"].ToString();
                            if (!reader["favorito"].Equals(DBNull.Value))
                                dto.favorito = Convert.ToInt32(reader["favorito"]);
                            dto.plataform = Convert.ToInt32(reader["plataform"]);
                            result = dto;
                        }
                    }
                }
            }
            return result;
        }

        public ScreenshotDTO spSelNextScreenshoot(int id, int? tipo, int? tipoGame, int? idGame)
        {
            ScreenshotDTO result = new ScreenshotDTO();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelNextScreenshoot", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idScreen", id);
                    command.Parameters.AddWithValue("@tipo", tipo);
                    command.Parameters.AddWithValue("@tipoGame", tipoGame);
                    command.Parameters.AddWithValue("@idGame", idGame);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ScreenshotDTO dto = new ScreenshotDTO();

                            dto.id = Convert.ToInt64(reader["id"]);
                            if (!reader["idGame"].Equals(DBNull.Value))
                                dto.idGame = Convert.ToInt32(reader["idGame"]);
                            dto.imagens = Convert.ToBase64String((byte[])reader["imagens"]);
                            if (!reader["icon"].Equals(DBNull.Value))
                                dto.icon = reader["icon"].ToString();
                            dto.nome = reader["nome"].ToString();
                            dto.descricao = reader["descricao"].ToString();
                            if (!reader["favorito"].Equals(DBNull.Value))
                                dto.favorito = Convert.ToInt32(reader["favorito"]);
                            result = dto;
                        }
                    }
                }
            }
            return result;
        }

        public List<PercentGameDTO> spSelPercentGamesForId(int id, int plataform)
        {
            List<PercentGameDTO> result = new List<PercentGameDTO>();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelPercentGamesForId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idGame", id);
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PercentGameDTO dto = new PercentGameDTO();

                            if (!reader["idGame"].Equals(DBNull.Value))
                                dto.idGame = Convert.ToInt32(reader["idGame"]);
                            dto.nome = reader["name"].ToString();
                            dto.icon = reader["icon"].ToString();
                            dto.porcentagem = Convert.ToDouble(reader["porcent"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<ScreenshotDTO> spSelScreenshootsPorIdGame(int id)
        {
            List<ScreenshotDTO> result = new List<ScreenshotDTO>();
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelScreenshootsPorIdGame", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ScreenshotDTO dto = new ScreenshotDTO();

                            if (!reader["idGame"].Equals(DBNull.Value))
                                dto.idGame = Convert.ToInt32(reader["idGame"]);
                            dto.imagens = Convert.ToBase64String((byte[])reader["imagens"]);
                            dto.quantidade = Convert.ToInt32(reader["quantidade_total"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        //public ScreenshotDTO spSelFavScreenshootPorId(int id)
        //{
        //    ScreenshotDTO result = new ScreenshotDTO();
        //    using (SqlConnection connection = new SqlConnection(connectString))
        //    {
        //        connection.Open();
        //        using (SqlCommand command = new SqlCommand("spSelFavScreenshootPorId", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@id", id);

        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    ScreenshotDTO dto = new ScreenshotDTO();

        //                    dto.id = Convert.ToInt64(reader["id"]);
        //                    dto.nome = reader["nome"].ToString();
        //                    dto.descricao = reader["descricao"].ToString();
        //                    result = dto;
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}

        public ProfileDTO spSelProfileSteam(int? plataform)
        {
            ProfileDTO dto = new ProfileDTO();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelProfileGames", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@plataform", plataform);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dto.id = Convert.ToInt64(reader["id"]);
                            dto.name = reader["nome"].ToString();
                            if (!reader["imagem"].Equals(DBNull.Value))
                                if (plataform == null)
                                    dto.avatar = Convert.ToBase64String((byte[])reader["imagem"]);
                                else
                                    dto.avatar = reader["imagem"].ToString();
                            if (!reader["allGames"].Equals(DBNull.Value))
                                dto.allGames = Convert.ToInt32(reader["allGames"]);
                            if (!reader["porcentagem"].Equals(DBNull.Value))
                                dto.porcentagem = Convert.ToInt32(reader["porcentagem"]);
                            if (!reader["timeGame"].Equals(DBNull.Value))
                                dto.timeGame = Convert.ToInt32(reader["timeGame"]);
                            if (!reader["allAchievements"].Equals(DBNull.Value))
                                dto.allAchievements = Convert.ToInt32(reader["allAchievements"]);
                            if (!reader["allPlatinas"].Equals(DBNull.Value))
                                dto.allPlatinas = Convert.ToInt32(reader["allPlatinas"]);
                        }
                    }
                }
            }
            return dto;
        }

        public ProfileDTO spSelProfiles(int plataform)
        {
            ProfileDTO dto = new ProfileDTO();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelProfiles", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dto.id = Convert.ToInt32(reader["idProfile"]);
                            dto.name = reader["nome"].ToString();
                            dto.profileUrl = reader["ProfileUrl"].ToString();
                            dto.avatar = reader["avatar"].ToString();
                            if (!reader["numConquistas"].Equals(DBNull.Value))
                                dto.allAchievements = Convert.ToInt32(reader["numConquistas"]);
                            if (!reader["porcentualAch"].Equals(DBNull.Value))
                                dto.porcentagem = Convert.ToInt32(reader["porcentualAch"]);
                            if (!reader["completos"].Equals(DBNull.Value))
                                dto.perfeitos = Convert.ToInt32(reader["completos"]);
                        }
                    }
                }
            }
            return dto;
        }

        public List<AchievementsDTO> spSelAchievementsSteam(int id, int? ativo)
        {
            List<AchievementsDTO> result = new List<AchievementsDTO>();

            if (ativo == null)
            {
                ativo = 2;
            }

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelAchievementsSteam", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@ativo", ativo);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AchievementsDTO dto = new AchievementsDTO();

                            dto.id = Convert.ToInt64(reader["id"]);
                            dto.idGame = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.descricao = reader["descricao"].ToString();
                            dto.iconLock = reader["iconLock"].ToString();
                            dto.iconUnlock = reader["iconUnlock"].ToString();
                            if (!reader["porcent"].Equals(DBNull.Value))
                                dto.porcent = Convert.ToDouble(reader["porcent"]);
                            dto.ativo = Convert.ToInt32(reader["ativo"]);
                            if (!reader["dateUnlock"].Equals(DBNull.Value))
                                dto.dateUnclock = Convert.ToDateTime(reader["dateUnlock"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public List<AchievementsDTO> spSelAchievementsRetro(int id, int? ativo)
        {
            List<AchievementsDTO> result = new List<AchievementsDTO>();

            if (ativo == null)
            {
                ativo = 2;
            }

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelAchievementsRetro", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@ativo", ativo);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AchievementsDTO dto = new AchievementsDTO();

                            dto.id = Convert.ToInt64(reader["id"]);
                            dto.idGame = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["titulo"].ToString();
                            dto.descricao = reader["descricao"].ToString();
                            dto.iconUnlock = reader["icon"].ToString();
                            dto.ativo = Convert.ToInt32(reader["ativo"]);
                            dto.porcent = Convert.ToInt32(reader["porcentagem"]);
                            if (!reader["dataObtido"].Equals(DBNull.Value))
                                dto.dateUnclock = Convert.ToDateTime(reader["dataObtido"]);
                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public AchievementsDTO spSelAchievementsSteamPorId(int id, int idGame)
        {
            AchievementsDTO result = new AchievementsDTO();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelAchievementsSteamPorId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@idGame", idGame);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            AchievementsDTO dto = new AchievementsDTO();

                            dto.id = Convert.ToInt64(reader["id"]);
                            dto.idGame = Convert.ToInt64(reader["idGame"]);
                            dto.nome = reader["nome"].ToString();
                            dto.iconLock = reader["iconLock"].ToString();
                            dto.iconUnlock = reader["iconUnlock"].ToString();
                            if (!reader["porcent"].Equals(DBNull.Value))
                                dto.porcent = Convert.ToDouble(reader["porcent"]);
                            dto.ativo = Convert.ToInt32(reader["ativo"]);
                            result = dto;
                        }
                    }
                }
            }
            return result;
        }

        public int spSelPercentAchievementsSteam(int id, int plataform)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelPercentAchievementsSteam", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idGame", id);
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = Convert.ToInt32(reader["porcentagem"]);
                        }
                    }
                }
            }
            return result;
        }

        public int spSelPercentGamesSteamCateg(string categoria)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelPercentGamesSteamCateg", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@categ", categoria);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = Convert.ToInt32(reader["porcentagem"]);
                        }
                    }
                }
            }
            return result;
        }

        public int spSelPorcentFranquia(string franquia)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelPorcentFranquia", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@franquia", franquia);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = Convert.ToInt32(reader["porcentagem"]);
                        }
                    }
                }
            }
            return result;
        }

        public int spSelAchievementsObtidosSteam(int id, int plataform)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelAchievementsObtidosSteam", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idGame", id);
                    command.Parameters.AddWithValue("@plataform", plataform);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = Convert.ToInt32(reader["numero"]);
                        }
                    }
                }
            }
            return result;
        }

        public string spselBannerGameSteamId(int id)
        {
            string result = "";

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spselBannerGameSteamId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@idGame", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader["banner"].Equals(DBNull.Value))
                                result = Convert.ToBase64String((byte[])reader["banner"]);
                        }
                    }
                }
            }
            return result;
        }

        public void saveImgProfile(byte[] imagem)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("saveImgProfile", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@imagem", imagem);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spUpdOrdenagemGames(int arrastado, int alvo)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spUpdOrdenagemGames", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@arrastado", arrastado);
                    cmd.Parameters.AddWithValue("@alvo", alvo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spUpdImgCustom(byte[] imagem, int idGame, int tipo, int plataform)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spUpdImgCustom", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@imagem", imagem);
                    cmd.Parameters.AddWithValue("@idGame", idGame);
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@plataform", plataform);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spUpdGamesSteam(int id, string categoria, byte[] imgHeader, int plataform)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spUpdGamesSteam", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@plataform", plataform);
                    if (imgHeader != null)
                    {
                        cmd.Parameters.AddWithValue("@imgHeader", imgHeader);
                    }
                    if (categoria != null && categoria != "Selecionar")
                    {
                        cmd.Parameters.AddWithValue("@categoria", categoria);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<MusicasDTO> spSelMusica()
        {
            List<MusicasDTO> result = new List<MusicasDTO>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("spSelMusicas", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MusicasDTO dto = new MusicasDTO();

                            dto.id = Convert.ToInt32(reader["id"]);
                            dto.nome = reader["nome"].ToString();
                            if (!reader["sound"].Equals(DBNull.Value))
                                dto.sound = Convert.ToBase64String((byte[])reader["sound"]);

                            result.Add(dto);
                        }
                    }
                }
            }
            return result;
        }

        public void spUpdAchievementsGames(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spUpdAchievementsGames", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idGame", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spDelGameSteam(int id, int exc)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spDelGameSteam", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idGame", id);
                    cmd.Parameters.AddWithValue("@stts", exc);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spdelNoteGame(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spdelNoteGame", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spDelMetaGame(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spDelMetaGame", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spUpdPercentagemGamesSteamForDelete()
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spUpdPercentagemGamesSteamForDelete", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void spUpdCategGamesSteam(string categoria, byte[] imagem)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("spUpdCategGamesSteam", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@categoria", categoria);
                    if (imagem != null)
                    {
                        cmd.Parameters.AddWithValue("@imagem", imagem);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
