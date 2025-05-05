using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using ProjectGames.DAL;
using ProjectGames.DTO;
using static System.Net.Mime.MediaTypeNames;

namespace ProjectGames.BLL
{
    public class GamesBLL
    {
        public void spInsGames(DadosGame listGames)
        {
            GamesDAL dal = new GamesDAL();

            dal.spInsGames(listGames);
        }

        public void spInsRetroGames(DadosRetro listRetro, int type, int? idGame)
        {
            GamesDAL dal = new GamesDAL();

            dal.spInsRetroGames(listRetro, type, idGame);
        }

        public void spInsProfileRetro(ProfileRetro profile)
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsProfileRetro(profile);
        }

        public void spInsPercentualRetroGames()
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsPercentualRetroGames();
        }

        public void spInsAchievementsRetroGames(DadosRetro listAchiev, int idGame)
        {
            GamesDAL dal = new GamesDAL();

            foreach (var item in listAchiev.Achievements)
            {
                item.Value.porcentagemAch = item.Value.NumAwarded / listAchiev.NumDistinctPlayers * 100;
            }
            dal.spInsAchievementsRetroGames(listAchiev, idGame);
        }

        public void spInsGamesListDesejo(List<SteamWishlistData> listDsj)
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsGamesListDesejo(listDsj);

        }

        public void spInsAchievementsGames(AchievementAll[] listAchievements, int id)
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsAchievementsGames(listAchievements, id);
        }

        public void spUpdSttsAchievementsGames(AchievementAll[] listAchievements, int idGame)
        {
            GamesDAL dal = new GamesDAL();
            dal.spUpdSttsAchievementsGames(listAchievements, idGame);
        }

        public void spInsPorcentagemGamesSteam(int id)
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsPorcentagemGamesSteam(id);
        }

        public void spInsDadosProfileSteam()
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsDadosProfileSteam();
        }

        public void spInsDestaqueAchSteam(int id, string achievement, int? idDestaque)
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsDestaqueAchSteam(id, achievement, idDestaque);
        }

        public void spInsMetaGame(int idGame, int ano, int mes)
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsMetaGame(idGame,ano,mes);
        }

        public void spInsNoteGame(string titulo, string mensagem, int idGame)
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsNoteGame(titulo, mensagem, idGame);

        }

        public void spUpdFavScreen(int id, int idScreen)
        {
            GamesDAL dal = new GamesDAL();
            dal.spUpdFavScreen(id, idScreen);
        }

        public void spInsFavoriteGameSteam(int idGame, int plataform)
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsFavoriteGameSteam(idGame, plataform);
        }

        public void spDelFavoriteGame(int idGame)
        {
            GamesDAL dal = new GamesDAL();
            dal.spDelFavoriteGame(idGame);
        }

        public void spInsScreenshotGame(string nome, string descricao, byte[] imagem, int? id, string pasta)
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsScreenshotGame(nome, descricao, imagem, id, pasta);

        }

        public void spInsItemFolder(string pasta)
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsItemFolder(pasta);
        }

        public void spInsFranchiseGameSteam(string title, int? idGame, int? tipo, string nomeFranquia)
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsFranchiseGameSteam(title, idGame, tipo, nomeFranquia);

        }

        public void spInsImgScreenShotItlg(byte[] ImgBytes, long? idGame, string nome, int plataform)
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsImgScreenShotItlg(ImgBytes, idGame, nome, plataform);
        }

        public void spInsfavoritarImgScreenShot(int id)
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsfavoritarImgScreenShot(id);
        }

        public void spInsMusica(string nome, byte[] file)
        {
            GamesDAL dal = new GamesDAL();
            dal.spInsMusica(nome, file);
        }

        public void spUpdGamesScreenshot(int id, int tipo, string mensagem, int plataform)
        {
            GamesDAL dal = new GamesDAL();
            dal.spUpdGamesScreenshot(id, tipo, mensagem, plataform);
        }

        public int spSelPlataformGamePorId(int id, int tipo)
        {
            GamesDAL dal = new GamesDAL();
            int result = 0;
            result = dal.spSelPlataformGamePorId(id, tipo);

            return result;
        }

        public ScreenshotDTO spSelScreenshootPorId(int id, int plataform)
        {
            GamesDAL dal = new GamesDAL();
            ScreenshotDTO dto = new ScreenshotDTO();

            dto = dal.spSelScreenshootPorId(id, plataform);

            return dto;
        }

        public ScreenshotDTO spSelNextScreenshoot(int id, int? tipo, int? tipoGame, int? idGame)
        {
            GamesDAL dal = new GamesDAL();
            ScreenshotDTO dto = new ScreenshotDTO();

            dto = dal.spSelNextScreenshoot(id, tipo, tipoGame, idGame);

            return dto;
        }

        public GamesDTO spSelPercentGamesForId(int id, int plataform)
        {
            GamesDAL dal = new GamesDAL();
            GamesDTO dto = new GamesDTO();

            dto.listRarest = dal.spSelPercentGamesForId(id, plataform);

            if (plataform == 1)
            {
                for (int i = 0; i <= dto.listRarest.Count - 1; i++)
                {
                    dto.listRarest[i].icon = dto.listRarest[i].icon + ".png";
                }
            }
            return dto;
        }

        public GamesDTO spSelScreenshootsPorIdGame(int id)
        {
            GamesDAL dal = new GamesDAL();
            GamesDTO dto = new GamesDTO();

            dto.listScreen = dal.spSelScreenshootsPorIdGame(id);

            return dto;
        }

        public GamesDTO spSelGamesSteam(int? tipo)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listGames = dal.spSelGamesSteam(tipo);

            return (dto);
        }

        public string spSelNameGame(int idGame, int plataform)
        {
            string obter = new GamesDAL().spSelNameGame(idGame, plataform);
            return obter;
        }

        public GamesDTO spSelGamesRetro()
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listGames = dal.spSelGamesRetro();

            for (int i = 0; i <= dto.listGames.Count - 1; i++)
            {
                dto.listGames[i].imagem = dto.listGames[i].imagem.Replace("/Images/", "Images/");
            }

            return (dto);
        }

        public GamesDTO spSelFranchiseGames()
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listGames = dal.spSelFranchiseGames();

            return (dto);
        }

        public GamesDTO spSelGameFranchiseId(string franquia, string plataform)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listGames = dal.spSelGameFranchiseId(franquia, plataform);

            return (dto);
        }

        public GamesDTO spSelGamesListGame(int? tipo, int? plataform,int layout,string search,string categ)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            if (search != null || categ != null)
            {
                if (search != null)
                {
                    dto.listJogosList = dal.spSelSearchListGame(search, plataform);
                }
                if(categ != null )
                {
                    dto.listJogosList = dal.spSelCategListGame(categ, plataform);
                }
            }
            else
            {
                dto.listJogosList = dal.spSelAllGamesList(tipo, plataform, layout);
            }
            return (dto);
        }

        public GamesDTO spSelListGamesRetro()
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listJogosList = dal.spSelListGamesRetro();

            return (dto);
        }

        public JogosDTO spSelGameRetroPorId(int idGame)
        {
            JogosDTO dto = new JogosDTO();
            GamesDAL dal = new GamesDAL();

            dto = dal.spSelGameRetroPorId(idGame);
            dto.imagem = dto.imagem.Replace("/Images/", "Images/");

            return dto;
        }

        public GamesDTO spSelGamesListGameCateg(string categ)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listJogosList = dal.spSelGamesListGameCateg(categ);

            return (dto);
        }

        public GamesDTO spSelForAchievements(int id, int? plataform)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();
            dto.listJogosList = dal.spSelForAchievements(id, plataform);
            return (dto);
        }

        public List<GamesLstDesejo> spselGamesListDesejo()
        {
            var obter = new GamesDAL().spselGamesListDesejo();
            return obter;
        }

        public StaticsGameList spSelStaticsGames(int? plataform)
        {
            GamesDAL dal = new GamesDAL();
            var obter = dal.spSelStaticsGames(plataform);

            dal.spUpdStaticsProfileGames(obter.porcentagem, obter.timeGame, obter.allAchievements, obter.allGames, obter.allPlatinas);
            return obter;
        }

        public JogosDTO spSelGamesPorId(int id, int plataforma)
        {
            JogosDTO dto = new JogosDTO();
            GamesDAL dal = new GamesDAL();

            dto = dal.spSelGamesPorId(id, plataforma);
            dto.screenshotGame = dal.spSelScreenshootsPorIdGame(id);

            return dto;
        }

        public GamesDTO spSelGamesSteamPorCateg(string categoria, string plataform)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listJogosList = dal.spSelGamesSteamPorCateg(categoria, plataform);

            return (dto);
        }

        public GamesDTO spSelGamesScreenPorFolder(int folder)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listGames = dal.spSelGamesScreenPorFolder(folder);

            return (dto);
        }

        public GamesDTO spSelGamesSteamForSearch(string search)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listGames = dal.spSelGamesSteamForSearch(search);
            return (dto);
        }

        public GamesDTO spSelAllGamesScreen()
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listGames = dal.spSelAllGamesScreen();
            return (dto);
        }

        public GamesDTO spselSlideGames(int? plataform,string identificador,int tipo)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();
            if (identificador == null || identificador == "")
            {
                dto.listGames = dal.spselSlideGames(plataform);
            }
            else
            {
                dto.listGames = dal.spselSlideGamesPorId(plataform, identificador,tipo);
            }
            return dto;
        }

        public GamesDTO spSelGamesSteamForPercentual(int porcent)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listGames = dal.spSelGamesSteamForPercentual(porcent);
            return (dto);
        }

        //public List<CategGamesDTO> spSelListGamesCateg()
        //{
        //    var obter = new GamesDAL().spSelListGamesCateg();
        //    return obter;
        //}

        public List<JogosDTO> spSelAllGamesSteam()
        {
            var obter = new GamesDAL().spSelGamesSteam(null);
            return obter;
        }

        public List<JogosDTO> spSelAllGames(int plataform)
        {
            var obter = new GamesDAL().spSelAllGames(plataform);
            return obter;
        }

        public AnotacoesDTO spSelNoteGame()
        {
            var obter = new GamesDAL().spSelNoteGame();
            return obter;
        }

        public List<PercentGameDTO> spSelRarestAchiev()
        {
            var obter = new GamesDAL().spSelRarestAchiev();
            for (int i = 0; i <= obter.Count - 1; i++)
            {
                if (obter[i].plataform == 1)
                {
                    obter[i].icon = obter[i].icon + ".png";
                }
            }
            return obter;
        }

        public List<FolderGamesDTO> spSelAllFolders()
        {
            var obter = new GamesDAL().spSelAllFolders();
            return obter;
        }

        public List<AchievementsDTO> spSelDestaqueAchSteam()
        {
            var obter = new GamesDAL().spSelDestaqueAchSteam();
            return obter;
        }

        public List<ScreenshotDTO> spSelFavScreenshoot()
        {
            var obter = new GamesDAL().spSelFavScreenshoot();
            return obter;
        }

        public List<ScreenshotDTO> spSelAllFavScreenshoot()
        {
            var obter = new GamesDAL().spSelAllFavScreenshoot();
            return obter;
        }

        public GamesDTO spSelMetaGames(int ano, int mes)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();
            dto.metaGames = new MetaGames();

            dto.listGames = dal.spSelMetaGames(ano,mes);
            dto.metaGames.ano = ano;
            dto.metaGames.mes = mes;
            return dto;
        }

        public List<FeedGames> spSelHistoricoGames(int? tipo)
        {
            List<FeedGames> dto = new List<FeedGames>();
            GamesDAL dal = new GamesDAL();

            dto = dal.spSelHistoricoGames(tipo);

            for (int i = 0; i <= dto.Count - 1; i++)
            {
                TimeSpan timeDifference = DateTime.Now - dto[i].dateObj;
                int monthsDifference = ((DateTime.Now.Year - dto[i].dateObj.Year) * 12) + DateTime.Now.Month - dto[i].dateObj.Month;

                if (timeDifference.TotalHours < 1)
                {
                    dto[i].timeElapsed = $"{(int)timeDifference.TotalMinutes} minutos atrás";
                }
                else if (timeDifference.TotalHours < 24)
                {
                    dto[i].timeElapsed = $"{(int)timeDifference.TotalHours} horas atrás";
                }
                else if (timeDifference.TotalDays < 30)
                {
                    dto[i].timeElapsed = $"{(int)timeDifference.TotalDays} dias atrás";
                }
                else if (monthsDifference <= 11)
                {
                    dto[i].timeElapsed = monthsDifference == 1 ? $"{monthsDifference} mês atrás" : $"{monthsDifference} meses atrás";
                }
                else
                {
                    int yearsDifference = DateTime.Now.Year - dto[i].dateObj.Year;
                    dto[i].timeElapsed = yearsDifference == 1 ? $"{yearsDifference} ano atrás" : $"{yearsDifference} anos atrás";
                }
            }
            return dto;
        }

        public FeedGames spSelLastCompletism()
        {
            GamesDAL dal = new GamesDAL();
            FeedGames dto = new FeedGames();
            dto = dal.spSelLastCompletism();

            TimeSpan timeDifference = DateTime.Now - dto.dateObj;
            int monthsDifference = ((DateTime.Now.Year - dto.dateObj.Year) * 12) + DateTime.Now.Month - dto.dateObj.Month;

            if (timeDifference.TotalHours < 1)
            {
                dto.timeElapsed = $"{(int)timeDifference.TotalMinutes} minutos atrás";
            }
            if (timeDifference.TotalHours > 1)
            {
                dto.timeElapsed = $"{(int)timeDifference.TotalHours} horas atrás";
            }
            if (timeDifference.TotalHours > 23)
            {
                dto.timeElapsed = $"{(int)timeDifference.TotalDays} dias atrás";
            }
            if (timeDifference.TotalDays > 29)
            {
                dto.timeElapsed = monthsDifference == 1 ? $"{monthsDifference} mês atrás" : $"{monthsDifference} meses atrás";
            }
            if (monthsDifference > 11)
            {
                int yearsDifference = DateTime.Now.Year - dto.dateObj.Year;
                dto.timeElapsed = yearsDifference == 1 ? $"{yearsDifference} ano atrás" : $"{yearsDifference} anos atrás";

            }
            return dto;
        }

        public GamesEstatisticas spSelDataStaticsGames()
        {
            GamesDAL dal = new GamesDAL();
            GamesEstatisticas dto = new GamesEstatisticas();

            dto = dal.spSeldataProfileStatistic();
            dto.listLP = dal.spSelDataStaticsGames();
            dto.listInd = dal.spSelIndiceGames(null);

            return dto;
        }

        public GamesDTO spSelFeedGames()
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listFeed = dal.spSelFeedGames();

            for (int i = 0; i <= dto.listFeed.Count - 1; i++)
            {
                TimeSpan timeDifference = DateTime.Now - dto.listFeed[i].dateObj;
                if (timeDifference.TotalHours < 1)
                {
                    dto.listFeed[i].timeElapsed = $"{(int)timeDifference.TotalMinutes} minutos atrás";
                }
                if (timeDifference.TotalHours < 24)
                {
                    dto.listFeed[i].timeElapsed = $"{(int)timeDifference.TotalHours} horas atrás";
                }
                else
                {
                    dto.listFeed[i].timeElapsed = $"{(int)timeDifference.TotalDays} dias atrás";
                }

            }
            return dto;
        }

        public GamesDTO spSelLastNotes()
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listFeed = dal.spSelLastNotes();
            for (int i = 0; i <= dto.listFeed.Count - 1; i++)
            {
                TimeSpan timeDifference = DateTime.Now - dto.listFeed[i].dateObj;
                if (timeDifference.TotalHours < 1)
                {
                    dto.listFeed[i].timeElapsed = $"{(int)timeDifference.TotalMinutes} minutos atrás";
                }
                if (timeDifference.TotalHours < 24)
                {
                    dto.listFeed[i].timeElapsed = $"{(int)timeDifference.TotalHours} horas atrás";
                }
                else
                {
                    dto.listFeed[i].timeElapsed = $"{(int)timeDifference.TotalDays} dias atrás";
                }
            }
            return dto;
        }

        public List<JogosDTO> spSelFavGamesSteam(int? tipo, int? pagina, int? size)
        {
            List<JogosDTO> obter = new List<JogosDTO>();
            if (tipo == 0)
            {
                obter = new GamesDAL().spSelFavGamesSteam(Convert.ToInt32(pagina));
            }
            if (tipo == 1)
            {
                obter = new GamesDAL().spSelFavGamesSteam(Convert.ToInt32(pagina));
            }
            if (tipo == null)
            {
                obter = new GamesDAL().spSelFavGamesSteam(1);
            }

            for (int i = 0; i <= obter.Count - 1; i++)
            {
                if (obter[i].imagem.Contains("/Images/"))
                {
                    obter[i].imagem = obter[i].imagem.Replace("/Images/", "https://media.retroachievements.org/Images/");
                }
            }
            return obter;
        }

        public int spSelCountSizeFavGame()
        {
            int obter = new GamesDAL().spSelCountSizeFavGame();
            obter = obter / 4;
            return obter;
        }

        public DadosGamesSteam spSelDadosGamesInAchievements(int id, int plataform)
        {
            var obter = new GamesDAL().spSelDadosGamesInAchievements(id, plataform);

            if (plataform == 1)
            {
                obter.LastAchiev = obter.LastAchiev + ".png";
            }
            return obter;
        }

        public ProfileDTO spSelProfileSteam(int?plataform)
        {
            ProfileDTO dto = new ProfileDTO();
            GamesDAL dal = new GamesDAL();

            dto = dal.spSelProfileSteam(plataform);

            return dto;
        }

        public ProfileDTO spSelProfiles(int plataform)
        {
            ProfileDTO dto = new ProfileDTO();
            GamesDAL dal = new GamesDAL();

            dto = dal.spSelProfiles(plataform);

            return dto;
        }

        public GamesDTO spSelAchievementsSteam(int id, int? ativo)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listAchievements = dal.spSelAchievementsSteam(id, ativo);
            return (dto);
        }

        public GamesDTO spSelAchievementsRetro(int id, int? ativo)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listAchievements = dal.spSelAchievementsRetro(id, ativo);

            for (int i = 0; i <= dto.listAchievements.Count - 1; i++)
            {
                dto.listAchievements[i].iconUnlock = dto.listAchievements[i].iconUnlock + ".png";
            }
            return (dto);
        }

        public AchievementsDTO spSelAchievementsSteamPorId(int id, int idGame)
        {
            AchievementsDTO dto = new AchievementsDTO();
            GamesDAL dal = new GamesDAL();

            dto = dal.spSelAchievementsSteamPorId(id, idGame);
            return (dto);

        }

        public int spSelPercentAchievements(int id, int plataform)
        {
            GamesDAL dal = new GamesDAL();
            int result = 0;
            result = dal.spSelPercentAchievementsSteam(id, plataform);

            return result;

        }

        public int spSelPercentGamesSteamCateg(string categoria)
        {
            GamesDAL dal = new GamesDAL();
            int result = 0;
            result = dal.spSelPercentGamesSteamCateg(categoria);

            return result;
        }

        public int spSelPorcentFranquia(string franquia)
        {
            GamesDAL dal = new GamesDAL();
            int result = 0;
            result = dal.spSelPorcentFranquia(franquia);
            return result;
        }

        public int spSelAchievementsObtidos(int id, int plataform)
        {
            GamesDAL dal = new GamesDAL();
            int result = 0;
            result = dal.spSelAchievementsObtidosSteam(id, plataform);

            return result;
        }

        public string spselBannerGamesId(int id)
        {
            GamesDAL dal = new GamesDAL();
            string result = "";
            result = dal.spselBannerGameSteamId(id);

            return result;
        }

        public GamesDTO getCategGamesSteam()
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listGames = dal.spSelCategGamesSteam();

            return dto;

        }

        public List<JogosDTO> spselCategoriasGames()
        {
            var obter = new GamesDAL().spselCategoriasGames();
            return obter;
        }

        public int spSelLastGameCateg(string categ)
        {
            int obter = new GamesDAL().spSelLastGameCateg(categ);
            return obter;
        }

        public GamesDTO spSelFoldersGames()
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listFolder = dal.spSelFoldersGames();


            return dto;
        }

        public GamesDTO spSelScreenshootGames(string plataform)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listScreen = dal.spSelScreenshootGames(plataform);

            return dto;
        }

        public GamesDTO spSelScreenshootGamesPorId(int idGame, int idFolder)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            if (idGame != null)
            {
                dto.listScreen = dal.spSelScreenshootGamesPorId(idGame);
            }
            if (idFolder != null && idFolder != 0)
            {
                dto.listScreen = dal.spSelScreenshootGamesPorFolder(idFolder);
            }

            return dto;
        }

        public GamesDTO spSelNotesGamesId(int idGame)
        {
            GamesDTO dto = new GamesDTO();
            GamesDAL dal = new GamesDAL();

            dto.listNote = dal.spSelNotesGamesId(idGame);

            return dto;
        }

        public List<MusicasDTO> spSelMusica()
        {
            List<MusicasDTO> dto = new List<MusicasDTO>();
            GamesDAL dal = new GamesDAL();

            dto = dal.spSelMusica();
            return dto;
        }

        public void removerScreenshot(int id)
        {
            GamesDAL dal = new GamesDAL();
            dal.spDelRemoveScreenshot(id);
        }

        public void saveImgProfile(byte[] imagem)
        {
            GamesDAL dal = new GamesDAL();
            dal.saveImgProfile(imagem);
        }

        public void spUpdOrdenagemGames(int arrastado, int alvo)
        {
            GamesDAL dal = new GamesDAL();
            dal.spUpdOrdenagemGames(arrastado, alvo);
        }

        public void spUpdImgCustom(byte[] imagem, int idGame, int tipo, int plataform)
        {
            GamesDAL dal = new GamesDAL();
            dal.spUpdImgCustom(imagem, idGame, tipo, plataform);
        }

        public void spUpdGamesSteam(int id, string categoria, byte[] imgHeader, int plataform)
        {
            GamesDAL dal = new GamesDAL();

            dal.spUpdGamesSteam(id, categoria, imgHeader, plataform);
        }

        public void spUpdAchievementsGames(int id)
        {
            GamesDAL dal = new GamesDAL();
            dal.spUpdAchievementsGames(id);
        }

        public void spDelGameSteam(int id, int exc)
        {
            GamesDAL dal = new GamesDAL();
            dal.spDelGameSteam(id, exc);
        }

        public void spdelNoteGame(int id)
        {
            GamesDAL dal = new GamesDAL();
            dal.spdelNoteGame(id);
        }

        public void spDelMetaGame(int id)
        {
            GamesDAL dal = new GamesDAL();
            dal.spDelMetaGame(id);
        }

        public void spUpdPercentagemGamesSteamForDelete()
        {
            GamesDAL dal = new GamesDAL();
            dal.spUpdPercentagemGamesSteamForDelete();
        }

        public void spUpdCategGamesSteam(string categoria, byte[] imagem)
        {
            GamesDAL dal = new GamesDAL();

            dal.spUpdCategGamesSteam(categoria, imagem);
        }

        public void spUpdEditNoteGame(int id, string mensagem)
        {
            GamesDAL dal = new GamesDAL();
            dal.spUpdEditNoteGame(id, mensagem);
        }
    }
}
