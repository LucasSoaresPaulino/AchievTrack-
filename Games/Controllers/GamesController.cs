using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectGames.DTO;
using ProjectGames.BLL;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProjectGames.DAL;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text;
using static System.Net.WebRequestMethods;
using System.Text.RegularExpressions;
using X.PagedList;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;
using Generico;
using DotNet.Highcharts;

namespace Games.Controllers
{
    public class GamesController : Controller
    {
        #region config
        public void configPlataform(string plataform)
        {
            HttpContext.Session.SetString("plataform", plataform);
        }
        public void configLayoutIndex(int layout)
        {
            HttpContext.Session.SetInt32("layout", layout);
        }
        public async Task<ActionResult> _PartialSideBarMusic()
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();
            dto = await ApiLofi();
            return PartialView(dto);

        }
        public ActionResult _PartialSidebar()
        {
            GamesBLL bll = new GamesBLL();
            ViewBag.Categoria = bll.spselCategoriasGames();

            return PartialView();
        }
        public ActionResult _PartialSkeleton(int? tipo)
        {
            ViewBag.tipo = tipo;
            return PartialView();
        }
        #endregion

        #region View

        #region Views

        public ActionResult Index(int? tipo, string categ)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelGamesSteam(tipo);

            ViewBag.Profile = bll.spSelProfileSteam(null);
            ViewBag.Categoria = bll.spselCategoriasGames();
            ViewBag.categ = categ;
            TempData["tipo"] = tipo;

            return View(dto);
        }

        public async Task<ActionResult> PageGame(int id, int? ativo, int? plataformIndex)
        {
            JogosDTO dto = new JogosDTO();
            GamesBLL bll = new GamesBLL();
            if (ativo == null)
            {
                ativo = 0;
            }
            int plataform = bll.spSelPlataformGamePorId(id, 1);

            dto = bll.spSelGamesPorId(id, plataform);

            ViewBag.Profile = bll.spSelProfileSteam(null);
            ViewBag.Type = ativo;
            ViewBag.PlataformIndex = plataformIndex;
            ViewBag.GameAPI = await BackgroundImgAPIGame(id);
            ViewBag.id = id;
            return View(dto);
        }

        public ActionResult Profile()
        {
            ProfileDTO dto = new ProfileDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelProfileSteam(null);

            ViewBag.Profile = bll.spSelProfileSteam(null);
            ViewBag.LastPorc = bll.spSelLastCompletism();
            ViewBag.FavScreenshots = bll.spSelAllFavScreenshoot();
            ViewBag.CountSizeFavGame = bll.spSelCountSizeFavGame();
            ViewBag.Banner = bll.spselSlideGames(null, null, 0);
            ViewBag.AllRareAchiev = bll.spSelRarestAchiev();
            ViewBag.Page = TempData["pagina"];
            var mes = DateTime.Now.ToString("MMMM", new CultureInfo("pt-BR"));
            ViewBag.Mes = mes;

            return View(dto);
        }

        public ActionResult GamesEstatisticas()
        {
            ProfileDTO dto = new ProfileDTO();
            GamesBLL bll = new GamesBLL();

            ViewBag.Profile = bll.spSelProfileSteam(null);
            ViewBag.LastPorc = bll.spSelLastCompletism();

            var categorias = new[] { "Jan", "Fev", "Mar", "Abr", "Mai" };
            var valores = new[] { 29.9, 71.5, 106.4, 129.2, 144.0 };

            ViewBag.Categorias = categorias;
            ViewBag.Valores = valores;

            return View();
        }

        public ActionResult GamesPercent(int porcent)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelGamesSteamForPercentual(porcent);
            var profile = bll.spSelProfileSteam(null);
            ViewBag.Profile = profile;
            ViewBag.completism = porcent;

            return View(dto);
        }

        public ActionResult RareAchievements()
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            var profile = bll.spSelProfileSteam(null);
            ViewBag.Profile = profile;

            ViewBag.AllRareAchiev = bll.spSelRarestAchiev();

            return View();
        }

        public ActionResult GamesCategoria()
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.getCategGamesSteam();
            ViewBag.Profile = bll.spSelProfileSteam(null);
            return View(dto);
        }

        public ActionResult GamesFranchise(string franquia)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelGameFranchiseId(franquia, HttpContext.Session.GetString("plataform"));
            ViewBag.Profile = bll.spSelProfileSteam(null);
            ViewBag.Banner = bll.spselSlideGames(null, franquia, 0);
            ViewBag.progresso = bll.spSelPorcentFranquia(franquia);
            return View(dto);
        }

        public async Task<ActionResult> jogosCateg(string categoria)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelGamesSteamPorCateg(categoria, HttpContext.Session.GetString("plataform"));
            ViewBag.Profile = bll.spSelProfileSteam(null);
            ViewBag.Porcentagem = bll.spSelPercentGamesSteamCateg(categoria);
            ViewBag.Banner = bll.spselSlideGames(null, categoria, 1);


            return View(dto);
        }

        public ActionResult GamesScreenshot(int? id)
        {
            GamesBLL bll = new GamesBLL();
            GamesDTO dto = new GamesDTO();

            dto = bll.spSelFoldersGames();

            ViewBag.Games = bll.spSelAllGamesSteam();
            ViewBag.Profile = bll.spSelProfileSteam(null);
            ViewBag.id = id;
            ViewBag.Folder = bll.spSelAllFolders();
            return View(dto);
        }

        public ActionResult IndexNote()
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelLastNotes();
            dto.listGames = bll.spSelAllGamesSteam();
            ViewBag.Profile = bll.spSelProfileSteam(null);
            ViewBag.lastNote = bll.spSelNoteGame();

            return View(dto);
        }

        public async Task<ActionResult> GamesAnotacoes(int idGame, string nome)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelNotesGamesId(idGame);
            ViewBag.Profile = bll.spSelProfileSteam(null);
            ViewBag.id = idGame;
            ViewBag.nome = nome;
            ViewBag.GameAPI = await BackgroundImgAPIGame(idGame);
            ViewBag.Games = bll.spSelAllGamesSteam();

            return View(dto);
        }

        public async Task<ActionResult> GamesSearch(string pesquisa)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();
            dto = await searchGames(pesquisa);
            ViewBag.Profile = bll.spSelProfileSteam(null);
            ViewBag.Pesquisa = pesquisa;
            return View(dto);
        }

        public ActionResult IndexHistorico(int? pagina, int? tipo)
        {
            int tamanhoPagina = 7;
            int numeroPagina = pagina ?? 1;

            List<FeedGames> dto = new List<FeedGames>();
            GamesBLL bll = new GamesBLL();
            dto = bll.spSelHistoricoGames(tipo);

            ViewBag.Profile = bll.spSelProfileSteam(null);
            ViewBag.tipo = tipo;

            return View(dto.ToPagedList(numeroPagina, tamanhoPagina));
        }

        #endregion

        #region Partial

        #region Index

        public ActionResult _PartialListaDesejo()
        {
            GamesLstDesejo dto = new GamesLstDesejo();
            GamesBLL bll = new GamesBLL();

            ViewBag.lstDesejo = bll.spselGamesListDesejo();

            return PartialView();
        }

        public ActionResult _PartialBanner(int? plataform,string identificador)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spselSlideGames(plataform, identificador, 0);

            return PartialView(dto);
        }

        public ActionResult _PartialSeetingsIndex(int? plataform, int? typeGrid)
        {
            GamesBLL bll = new GamesBLL();

            ViewBag.Statics = bll.spSelStaticsGames(plataform);
            ViewBag.categoria = bll.spselCategoriasGames();
            ViewBag.plataform = plataform;
            ViewBag.profile = bll.spSelProfileSteam(plataform);
            ViewBag.layout = Convert.ToInt32(HttpContext.Session.GetInt32("layout"));
            //ViewBag.typeGrid = typeGrid;

            return PartialView();
        }

        public ActionResult _PartialListaGamesIndex(int? tipo, string categ, string search, int? plataform)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelGamesListGame(tipo, plataform, Convert.ToInt32(HttpContext.Session.GetInt32("layout")), search, categ);
            ViewBag.layout = Convert.ToInt32(HttpContext.Session.GetInt32("layout"));
            return PartialView(dto);
        }

        public ActionResult _PartialLoadAchievements(int id, int? plataform)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();
            dto = bll.spSelForAchievements(id, plataform);
            return PartialView(dto);
        }

        public ActionResult _PartialCatalogGameIndex(int? tipo, int? plataform, string search, string categ)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelGamesListGame(tipo, plataform, Convert.ToInt32(HttpContext.Session.GetInt32("layout")), search, categ);
            return PartialView(dto);
        }

        public ActionResult _PartialSetting()
        {
            return PartialView();
        }

        public ActionResult _PartialEditGames(int id, int plataform)
        {
            JogosDTO dto = new JogosDTO();
            GamesBLL bll = new GamesBLL();
            plataform = bll.spSelPlataformGamePorId(id, 1);
            dto = bll.spSelGamesPorId(id, plataform);

            ViewBag.categoria = bll.spselCategoriasGames();
            return PartialView(dto);
        }

        public async Task<ActionResult> _PartialImgGrid(int idGame, int tipo, int plataform, string endpoint)
        {
            GridGames dto = new GridGames();
            GamesBLL bll = new GamesBLL();
            ViewBag.tipo = tipo;
            ViewBag.idGame = idGame;
            ViewBag.plataform = plataform;

            HttpClient client = new HttpClient();
            if (tipo == 0)
            {
                endpoint = $"https://www.steamgriddb.com/api/v2/grids/game/{await idApiGrid(Convert.ToString(bll.spSelNameGame(idGame, plataform)), 0)}?dimensions=600x900";
            }
            if (tipo == 1)
            {
                endpoint = $"https://www.steamgriddb.com/api/v2/heroes/game/{await idApiGrid(Convert.ToString(bll.spSelNameGame(idGame, plataform)), 0)}?dimensions=1920x620";
            }
            if (tipo == 2)
            {
                endpoint = $"https://www.steamgriddb.com/api/v2/grids/game/{await idApiGrid(Convert.ToString(bll.spSelNameGame(idGame, plataform)), 0)}?dimensions=460x215";
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "c57abf417069595edd3073dc6922b862");
            HttpResponseMessage response = await client.GetAsync(endpoint);
            string responseData = await response.Content.ReadAsStringAsync();

            dto = JsonConvert.DeserializeObject<GridGames>(responseData);
            ViewBag.AppId = await idApiGrid(Convert.ToString(bll.spSelNameGame(idGame, plataform)), 0);
            ViewBag.nome = bll.spSelNameGame(idGame, plataform);

            return PartialView(dto);
        }

        public ActionResult _PartialGamesForSearch(string search, int tipo)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            if (tipo == 0)
            {
                dto = bll.spSelGamesSteamForSearch(search);
            }
            if (tipo == 1)
            {
                dto = bll.spSelAllGamesScreen();
            }
            ViewBag.tipo = tipo;

            return View(dto);
        }

        public ActionResult _PartialGamesForCategory(string categoria)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelGamesSteamPorCateg(categoria, null);
            ViewBag.categoria = bll.spselCategoriasGames();
            return View(dto);
        }

        #endregion

        #region PageGame

        public async Task<ActionResult> _PartialPageGame(int id)
        {
            JogosDTO dto = new JogosDTO();
            GamesBLL bll = new GamesBLL();

            int plataform = bll.spSelPlataformGamePorId(id, 1);

            var dadosAPI = await GameExtended(id, plataform);
            dto = bll.spSelGamesPorId(id, plataform);
            dto.imgHeader = dadosAPI.imgHeader;
            dto.descricao = dadosAPI.descricao;

            ViewBag.Profile = bll.spSelProfileSteam(null);
            ViewBag.GameAPI = await BackgroundImgAPIGame(id);
            return PartialView(dto);
        }

        public async Task<ActionResult> _PartialPageNewGame(int id, int plataform,int?tipo)
        {
            JogosDTO dto = new JogosDTO();
            GamesBLL bll = new GamesBLL();

            dto = await GameExtended(id, plataform);

            ViewBag.Profile = bll.spSelProfileSteam(null);
            ViewBag.GameAPI = await BackgroundImgAPIGame(id);
            ViewBag.tipo = tipo;
            return PartialView(dto);
        }

        public ActionResult _PartialAchievementsPage(int idGame, int? ativo, int plataform)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            if (plataform == 0)
            {
                dto = bll.spSelAchievementsSteam(idGame, ativo);
            }
            else
            {
                dto = bll.spSelAchievementsRetro(idGame, ativo);
            }

            ViewBag.plataform = plataform;
            return PartialView(dto);
        }

        public ActionResult _PartialRarestAchForId(string idGame, int plataform)
        {
            GamesBLL bll = new GamesBLL();
            GamesDTO dto = new GamesDTO();
            dto = bll.spSelPercentGamesForId(Convert.ToInt32(idGame), plataform);
            ViewBag.plataform = plataform;
            return PartialView(dto);
        }

        public ActionResult _PartialScreenGame(string idGame)
        {
            GamesBLL bll = new GamesBLL();
            GamesDTO dto = new GamesDTO();
            dto = bll.spSelScreenshootsPorIdGame(Convert.ToInt32(idGame));
            return PartialView(dto);
        }

        #endregion

        #region Screenshot

        public ActionResult _PartialScreenshot(int id, int? tipo, int? tipoGame, int? idGame, int plataform)
        {
            ScreenshotDTO dto = new ScreenshotDTO();
            GamesBLL bll = new GamesBLL();
            //int plataform = bll.spSelPlataformGamePorId(id,2);
            if (tipo == null)
            {
                dto = bll.spSelScreenshootPorId(id, plataform);
            }
            else
            {
                dto = bll.spSelNextScreenshoot(id, tipo, tipoGame, idGame);
            }
            ViewBag.Games = bll.spSelAllGames(plataform);
            ViewBag.Folder = bll.spSelAllFolders();

            return PartialView(dto);
        }

        public ActionResult _PartialScreenshotGerenc(int tipo, int idGame, int idFolder, string plataform)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();
            if (tipo == 0)
            {
                dto = bll.spSelScreenshootGames(HttpContext.Session.GetString("plataform"));
            }
            else
            {
                dto = bll.spSelScreenshootGamesPorId(idGame, idFolder);
            }
            ViewBag.tipo = tipo;
            ViewBag.idGame = idGame;
            return PartialView(dto);
        }

        public ActionResult _PartialGamesForFolder(int folder)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelGamesScreenPorFolder(folder);

            return PartialView(dto);
        }

        public ActionResult _PartialFoldersGerenc()
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelFoldersGames();

            return PartialView(dto);
        }

        #endregion

        #region profile

        public ActionResult _PartialDestaques(int id)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelAchievementsSteam(id, 2);
            return PartialView(dto);
        }

        public ActionResult _PartialMetaGames(int ano, int mes)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelMetaGames(ano, mes);
            return PartialView(dto);
        }

        public ActionResult _PartialFeedGames()
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();
            ViewBag.Profile = bll.spSelProfileSteam(null);

            dto = bll.spSelFeedGames();
            return PartialView(dto);
        }

        public ActionResult _PartialFavGamesProfile(int? tipo, int? pagina)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();
            int size = bll.spSelCountSizeFavGame();
            ViewBag.CountSizeFavGame = bll.spSelCountSizeFavGame();
            pagina = pagina ?? 1;
            if (tipo == 1)
            {
                if (pagina < size)
                {
                    pagina = pagina + 1;
                    dto.listGames = bll.spSelFavGamesSteam(tipo, pagina, size);
                }
            }
            if (tipo == 0)
            {
                if (pagina > 1)
                {
                    pagina = pagina - 1;
                }
                if (pagina >= 1)
                {
                    dto.listGames = bll.spSelFavGamesSteam(tipo, pagina, size);
                }
            }
            ViewBag.Page = pagina;
            if (tipo == null)
            {
                dto.listGames = bll.spSelFavGamesSteam(tipo, pagina, size);
            }
            TempData["pagina"] = pagina;
            TempData["size"] = size;
            return PartialView(dto);
        }

        #endregion

        #region Anotações

        public PartialViewResult _PartialAchievementNote(int id, int idGame)
        {
            AchievementsDTO dto = new AchievementsDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelAchievementsSteamPorId(id, idGame);

            return PartialView(dto);
        }

        #endregion

        #region Categoria

        public PartialViewResult _PartialCategGames()
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.getCategGamesSteam();
            ViewBag.Profile = bll.spSelProfileSteam(null);
            return PartialView(dto);
        }

        public PartialViewResult _PartialFranchiseGames()
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelFranchiseGames();

            ViewBag.Games = bll.spSelAllGamesSteam();
            return PartialView(dto);
        }

        public PartialViewResult _PartialResultSearchGames(string pesquisa)
        {
            GamesDTO dto = new GamesDTO();
            GamesBLL bll = new GamesBLL();

                dto = bll.spSelGamesListGame(null,null,0,pesquisa,null);
            
            return PartialView(dto);
        }

        #endregion

        #region Estatisticas

        public PartialViewResult _PartialEstatistica(int plataform)
        {
            GamesEstatisticas dto = new GamesEstatisticas();
            GamesBLL bll = new GamesBLL();

            dto = bll.spSelDataStaticsGames();           
            return PartialView(dto);
        }

        #endregion
        #endregion

        #endregion

        #region Save

        public void SaveDraginDrop(int arrastado, int alvo)
        {
            GamesBLL bll = new GamesBLL();
            bll.spUpdOrdenagemGames(arrastado, alvo);
        }

        public void saveImgProfile(IFormFile imagem)
        {
            GamesBLL bll = new GamesBLL();
            byte[] file = null;
            file = genBytes(imagem);

            bll.saveImgProfile(file);
        }

        public async void saveGridimagem(int idGame, string imagem, int tipo, int plataform)
        {
            GamesBLL bll = new GamesBLL();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(imagem);

                var teste = response.Content.ReadAsByteArrayAsync();
                byte[] img = response.Content.ReadAsByteArrayAsync().Result;
                bll.spUpdImgCustom(img, idGame, tipo, plataform);
            }
        }

        public void SaveEdit(int id, int plataform, string categoria, IFormFile imgBox, int exclude)
        {
            GamesBLL bll = new GamesBLL();
            byte[] imgHeader = null;

            if (imgBox != null)
            {
                imgHeader = genBytes(imgBox);
            }
            bll.spUpdGamesSteam(id, categoria, imgHeader, plataform);
            //if (exclude != null)
            //{
            //    bll.spDelGameSteam(id, exclude);
            //    bll.spUpdPercentagemGamesSteamForDelete();
            //}
        }

        public void favoriteGame(int idGame, int tipo, int plataform)
        {
            GamesBLL bll = new GamesBLL();
            if (tipo == 0)
            {
                bll.spInsFavoriteGameSteam(idGame, plataform);
            }
            else
            {
                bll.spDelFavoriteGame(idGame);
            }
        }

        public void saveDestaque(int id, string achievement, int? idDestaque)
        {
            GamesBLL bll = new GamesBLL();
            bll.spInsDestaqueAchSteam(id, achievement, idDestaque);
        }

        public void saveMetaGame(int idGame, int ano, int mes)
        {
            GamesBLL bll = new GamesBLL();
            bll.spInsMetaGame(idGame,ano,mes);
        }

        public void saveEditFavScreen(int id, int idScreen)
        {
            GamesBLL bll = new GamesBLL();
            bll.spUpdFavScreen(id, idScreen);
        }

        public void SaveEditCateg(string categoria, IFormFile imagem)
        {
            GamesBLL bll = new GamesBLL();
            byte[] file = null;
            if (imagem != null)
            {
                file = genBytes(imagem);
            }
            bll.spUpdCategGamesSteam(categoria, file);
        }

        public void saveScreen(string nome, string descricao, IFormFile imagem, int? id, string pasta)
        {
            GamesBLL bll = new GamesBLL();
            byte[] file = null;
            if (imagem != null)
            {
                file = genBytes(imagem);
            }
            bll.spInsScreenshotGame(nome, descricao, file, id, pasta);
        }

        public void saveFolder(string folder)
        {
            GamesBLL bll = new GamesBLL();
            bll.spInsItemFolder(folder);
        }

        public void favoritarImg(int id)
        {
            GamesBLL bll = new GamesBLL();
            bll.spInsfavoritarImgScreenShot(id);
        }

        public void addImgInteligent()
        {
            GamesBLL bll = new GamesBLL();
            GamesDTO dto = new GamesDTO();
            dto = bll.spSelGamesSteam(null);

            foreach (var item in dto.listGames)
            {
                string pasta = @"C:\Program Files (x86)\Steam\userdata\1245023787\760\remote\variavel\screenshots";
                pasta = pasta.Replace("variavel", item.id.ToString());

                if (Directory.Exists(pasta))
                {
                    // Obtém todas as imagens na pasta
                    string[] imagens = ObterImagensNaPasta(pasta);

                    foreach (string caminhoImagem in imagens)
                    {
                        // Transforma a imagem em byte[]
                        byte[] bytesDaImagem = LerBytesDaImagem(caminhoImagem);
                        bll.spInsImgScreenShotItlg(bytesDaImagem, item.id, item.nome, 0);
                    }
                }
            }
            addImgInteligentRetro();
        }

        public void addImgInteligentRetro()
        {
            GamesBLL bll = new GamesBLL();

            string pasta = @"D:\Imagens\Pics\ScreenRetroG";

            if (Directory.Exists(pasta))
            {
                // Obtém todas as imagens na pasta
                string[] imagens = ObterImagensNaPasta(pasta);

                foreach (string caminhoImagem in imagens)
                {
                    // Transforma a imagem em byte[]
                    byte[] bytesDaImagem = LerBytesDaImagem(caminhoImagem);
                    bll.spInsImgScreenShotItlg(bytesDaImagem, null, "", 1);
                }
            }

        }

        public void saveAltScreenshot(int id, int tipo, string mensagem, int plataform)
        {
            GamesBLL bll = new GamesBLL();
            bll.spUpdGamesScreenshot(id, tipo, mensagem, plataform);
        }

        public void SaveNoteGame(string titulo, string mensagem, int idGame)
        {
            GamesBLL bll = new GamesBLL();
            mensagem = mensagem.Replace(" ", "&nbsp;");

            bll.spInsNoteGame(titulo, mensagem, idGame);
        }

        public void editNoteGame(int id, string mensagem)
        {
            GamesBLL bll = new GamesBLL();
            mensagem = mensagem.Replace(" ", "&nbsp;");

            bll.spUpdEditNoteGame(id, mensagem);
        }

        public void saveFranchiseGame(string title, string games, int? tipo, string nomeFranquia)
        {
            GamesBLL bll = new GamesBLL();
            if (games != null)
            {
                string[] jogos = games.Split(',');           
                foreach (var item in jogos)
                {
                    bll.spInsFranchiseGameSteam(title, Convert.ToInt32(item), tipo, nomeFranquia);
                }
            }
            else
            {
                bll.spInsFranchiseGameSteam(title, null, tipo, nomeFranquia);
            }
        }

        public void saveMp3(IFormFile mp3File)
        {
            GamesBLL bll = new GamesBLL();

            string nome = mp3File.FileName;
            byte[] file = null;
            if (mp3File != null)
            {
                file = genBytes(mp3File);
            }
            bll.spInsMusica(nome, file);
        }

        #endregion

        #region Remove

        public void removeScreenshot(string screen)
        {
            GamesBLL bll = new GamesBLL();

            string[] imagens = screen.Split(',');

            foreach (var item in imagens)
            {
                bll.removerScreenshot(Convert.ToInt32(item));
            }
        }

        public void removeNoteGame(int id)
        {
            GamesBLL bll = new GamesBLL();
            bll.spdelNoteGame(id);
        }

        public void removeMetaGame(int id)
        {
            GamesBLL bll = new GamesBLL();
            bll.spDelMetaGame(id);
        }

        #endregion

        #region Formatação

        public byte[] genBytes(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.OpenReadStream().CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static string[] ObterImagensNaPasta(string pasta)
        {
            // Filtra os arquivos da pasta para obter apenas os arquivos de imagem
            return Directory.GetFiles(pasta, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => s.EndsWith(".jpg") || s.EndsWith(".jpeg") || s.EndsWith(".png") || s.EndsWith(".gif"))
                .ToArray();
        }

        public static byte[] LerBytesDaImagem(string caminho)
        {
            using (FileStream fs = new FileStream(caminho, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    return br.ReadBytes((int)fs.Length);
                }
            }
        }

        private string SanitizeString(string input)
        {
            var result = new StringBuilder();
            var numbers = new StringBuilder();
            var upperChars = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsUpper(c))
                    upperChars.Append(c);
                else if (char.IsDigit(c))
                    numbers.Append(c);
            }

            result.Append(upperChars).Append(numbers);
            return result.ToString();
        }

        #endregion

        #region API

        #region DadosGames

        public async void refreshStatsSteam()
        {
            GamesBLL bll = new GamesBLL();
            HttpClient client = new HttpClient();

            var steamResponse = await client.GetAsync($"https://api.steampowered.com/IPlayerService/GetRecentlyPlayedGames/v0001/?key={Conexao.ConectarAPI(1)}&format=json");
            var steamResult = steamResponse.Content.ReadAsStringAsync().Result;
            var ListId = JsonConvert.DeserializeObject<DadosGame>(steamResult);

            bll.spInsGames(ListId);

            //wishlistdata();
            uploadAchievements(ListId);
            addImgInteligent();
            refreshStatsRetro();
        }

        public async void refreshStatsRetro()
        {
            GamesBLL bll = new GamesBLL();
            HttpClient client = new HttpClient();

            //Profile
            var retroResponse = await client.GetAsync($"https://retroachievements.org/API/API_GetUserProfile.php?{Conexao.ConectarAPI(2)}");
            var retroResult = retroResponse.Content.ReadAsStringAsync().Result;
            var Profile = JsonConvert.DeserializeObject<ProfileRetro>(retroResult);

            bll.spInsProfileRetro(Profile);

            //Games
            var GretroResponse = await client.GetAsync($"https://retroachievements.org/API/API_GetUserCompletionProgress.php?{Conexao.ConectarAPI(2)}");
            var GretroResult = GretroResponse.Content.ReadAsStringAsync().Result;
            var ListGamesR = JsonConvert.DeserializeObject<DadosRetro>(GretroResult);

            bll.spInsRetroGames(ListGamesR, 0, null);

            //Achievements
            uploadAchievementsRetro(ListGamesR);

            //porcentual
            bll.spInsPercentualRetroGames();
        }

        public async void uploadAchievementsRetro(DadosRetro listId)
        {
            GamesBLL bll = new GamesBLL();
            HttpClient client = new HttpClient();

            foreach (var item in listId.Results)
            {
                var id = item.GameID;
                var response = await client.GetAsync($"https://retroachievements.org/API/API_GetGameInfoAndUserProgress.php?{Conexao.ConectarAPI(2)}&g={id}"); //API que retorna os dados da conquista
                var result = response.Content.ReadAsStringAsync().Result;
                var listAchievmentsRetro = JsonConvert.DeserializeObject<DadosRetro>(result);

                bll.spInsRetroGames(listAchievmentsRetro, 1, id);
                bll.spInsAchievementsRetroGames(listAchievmentsRetro, id);
            }
        }

        public async void uploadAchievements(DadosGame ListId)
        {
            GamesBLL bll = new GamesBLL();
            HttpClient client = new HttpClient();

            foreach (var item in ListId.Response.Games) //id re4 254700 
            {
                var id = item.Appid;
                var response = await client.GetAsync($"http://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v0002/?key={Conexao.ConectarAPI(1)}&appid={id}&l=english&format=json"); //API que retorna os dados da conquista
                var validateAch = await client.GetAsync($"https://api.steampowered.com/ISteamUserStats/GetPlayerAchievements/v0001/?appid={id}&key={Conexao.ConectarAPI(1)}&l=english"); //API que retorna as conquistas desbloqueadas
                var RespPorcent = await client.GetAsync($"https://api.steampowered.com/ISteamUserStats/GetGlobalAchievementPercentagesForApp/v0002/?gameid={id}&format=json");

                if (response.IsSuccessStatusCode && validateAch.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    var resultValidate = validateAch.Content.ReadAsStringAsync().Result;
                    var percentResult = RespPorcent.Content.ReadAsStringAsync().Result;

                    var listAchievments = JsonConvert.DeserializeObject<DadosGame>(result);
                    var listvalidateAchievments = JsonConvert.DeserializeObject<DadosGame>(resultValidate);
                    var listPercentAchievments = JsonConvert.DeserializeObject<DadosGame>(percentResult);

                    for (int i = 0; i <= listAchievments.Game.AvailableGameStats.Achievementall.Length - 1; i++)
                    {
                        try
                        {
                            listAchievments.Game.AvailableGameStats.Achievementall[i].Obtido = Convert.ToString(listvalidateAchievments.playStats.Achievements[i].Achieved);
                            listAchievments.Game.AvailableGameStats.Achievementall[i].DateUnlock = (listvalidateAchievments.playStats.Achievements[i].date);

                        }
                        catch
                        {
                            listAchievments.Game.AvailableGameStats.Achievementall[i].Obtido = "0";
                            listAchievments.Game.AvailableGameStats.Achievementall[i].DateUnlock = 0;
                        }
                    }

                    int cont = 0;

                    for (int i = 0; i <= listPercentAchievments.achievPercent.PercentAchievement.Length - 1;)
                    {
                        if (listAchievments.Game.AvailableGameStats.Achievementall[cont].refName == listPercentAchievments.achievPercent.PercentAchievement[i].name)
                        {
                            listAchievments.Game.AvailableGameStats.Achievementall[cont].porcent = listPercentAchievments.achievPercent.PercentAchievement[i].porcentual;
                            i++;
                            cont = 0;
                        }
                        else
                        {
                            cont++;
                        }
                    }

                    bll.spInsAchievementsGames(listAchievments.Game.AvailableGameStats.Achievementall, Convert.ToInt32(item.Appid));
                    bll.spInsPorcentagemGamesSteam(Convert.ToInt32(item.Appid));
                    bll.spInsDadosProfileSteam();
                }
            }
        }

        public async void reRunAchievements(int idGame)
        {
            HttpClient client = new HttpClient();

            GamesBLL bll = new GamesBLL();
            bll.spUpdAchievementsGames(idGame);

            var response = await client.GetAsync($"http://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v0002/?key={Conexao.ConectarAPI(1)}&appid={idGame}&l=english&format=json");
            var RespPorcent = await client.GetAsync($"https://api.steampowered.com/ISteamUserStats/GetGlobalAchievementPercentagesForApp/v0002/?gameid={idGame}&format=json");

            var result = response.Content.ReadAsStringAsync().Result;
            var percentResult = RespPorcent.Content.ReadAsStringAsync().Result;

            var listAchievments = JsonConvert.DeserializeObject<DadosGame>(result);
            var listPercentAchievments = JsonConvert.DeserializeObject<DadosGame>(percentResult);

            int cont = 0;

            for (int i = 0; i <= listPercentAchievments.achievPercent.PercentAchievement.Length - 1;)
            {
                if (listAchievments.Game.AvailableGameStats.Achievementall[cont].refName == listPercentAchievments.achievPercent.PercentAchievement[i].name)
                {
                    listAchievments.Game.AvailableGameStats.Achievementall[cont].porcent = listPercentAchievments.achievPercent.PercentAchievement[i].porcentual;
                    i++;
                    cont = 0;
                }
                else
                {
                    cont++;
                }
            }
            bll.spUpdSttsAchievementsGames(listAchievments.Game.AvailableGameStats.Achievementall, idGame);
        }

        //public async void wishlistdata()
        //{
        //    string endpoint = $"https://store.steampowered.com/wishlist/profiles/76561199205289515/wishlistdata/?p=0";

        //    using (HttpClient client = new HttpClient())
        //    {
        //        HttpResponseMessage response = await client.GetAsync(endpoint);
        //        string responseData = await response.Content.ReadAsStringAsync();
        //        List<SteamWishlistData> wishlistData = ParseWishlistData(responseData);
        //    }
        //}

        //private List<SteamWishlistData> ParseWishlistData(string jsonData)
        //{
        //    GamesBLL bll = new GamesBLL();

        //    JObject wishlistJson = JObject.Parse(jsonData);
        //    List<SteamWishlistData> wishlistDataList = new List<SteamWishlistData>();

        //    foreach (var item in wishlistJson)
        //    {
        //        SteamWishlistData wishlistItem = new SteamWishlistData
        //        {
        //            AppId = item.Key,
        //            Name = item.Value["name"]?.ToString(),
        //            Imagem = item.Value["capsule"]?.ToString(),
        //            Review = item.Value["review_desc"]?.ToString(),
        //            Date = Convert.ToInt64(item.Value["release_date"])
        //        };

        //        wishlistDataList.Add(wishlistItem);
        //    }
        //    bll.spInsGamesListDesejo(wishlistDataList);
        //    return wishlistDataList;
        //}

        #endregion

        #region imgGrid

        public async Task<int> idApiGrid(string nameGame, int AppId)
        {
            HttpClient client = new HttpClient();
            string endpoint = $"https://www.steamgriddb.com/api/v2/search/autocomplete/{nameGame}";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "c57abf417069595edd3073dc6922b862");
            HttpResponseMessage response = await client.GetAsync(endpoint);
            string responseData = await response.Content.ReadAsStringAsync();
            JObject gameListJson = JObject.Parse(responseData);
            JArray dataArray = gameListJson["data"] as JArray;

            AppId = Convert.ToInt32(dataArray[0]["id"]);
            return AppId;
        }

        public async Task<List<GameDetails>> BackgroundImgAPIGame(int id)
        {
            string endpoint = $"https://store.steampowered.com/api/appdetails?appids={id}&l=portuguese";
            List<GameDetails> parsedData = new List<GameDetails>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(endpoint);
                string responseData = await response.Content.ReadAsStringAsync();
                parsedData = EndGameAPI(responseData);
            }
            return parsedData;
        }
        private List<GameDetails> EndGameAPI(string jsonData)
        {
            GamesBLL bll = new GamesBLL();

            JObject GamelistJson = JObject.Parse(jsonData);
            List<GameDetails> GamelistDataList = new List<GameDetails>();

            foreach (var item in GamelistJson)
            {
                GameDetails gameItem = new GameDetails
                {
                    AppId = item.Key,
                };

                GamelistDataList.Add(gameItem);
            }

            if (GamelistJson.ContainsKey(GamelistDataList[0].AppId))
            {
                JObject appDetails = GamelistJson[GamelistDataList[0].AppId]?["data"] as JObject;
                if (appDetails != null)
                {
                    GamelistDataList[0].BackgroundRaw = appDetails.Value<string>("background");
                    GamelistDataList[0].descricao = appDetails.Value<string>("short_description");
                    if (appDetails["metacritic"] != null)
                    {
                        GamelistDataList[0].metaCritic = appDetails["metacritic"]["score"].Value<int>();
                    }
                }
            }
            return GamelistDataList;
        }

        #endregion

        #region search

        public async Task<GamesDTO> searchGames(string pesquisa)
        {
            GamesDTO dto = new GamesDTO();
            dto.listSearch = new List<GamesSearch>(); // Inicialize a lista aqui

            HttpClient client = new HttpClient();
            //STEAM
            string endpointSteam = $"https://store.steampowered.com/api/storesearch/?term={pesquisa}&l=english&cc=US ";
            HttpResponseMessage response = await client.GetAsync(endpointSteam);
            string responseData = await response.Content.ReadAsStringAsync();
            JObject gameListJson = JObject.Parse(responseData);
            JArray dataArray = gameListJson["items"] as JArray;

            if (dataArray != null)
            {
                foreach (var item in dataArray)
                {
                    dto.listSearch.Add(new GamesSearch
                    {
                        id = Convert.ToInt32(item["id"]),
                        nome = Convert.ToString(item["name"]),
                        imagem = Convert.ToString("https://cdn.cloudflare.steamstatic.com/steam/apps/" + Convert.ToInt32(item["id"]) + "/header.jpg"),
                        console = "Steam",
                        plataforma = 0
                    });
                }
            }
            //RETRO
            //string endpointRetro = "https://retroachievements.org/API/API_GetGameList.php?{Conexao.ConectarAPI(2)}&i=21&f=1";
            //using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            //{
            //    try
            //    {
            //        HttpResponseMessage responseR = await client.GetAsync(endpointRetro, cancellationTokenSource.Token);
            //        string responseDataR = await responseR.Content.ReadAsStringAsync(cancellationTokenSource.Token);
            //        JArray dataArrayR = JArray.Parse(responseDataR);

            //        if (dataArrayR != null)
            //        {
            //            List<JToken> filteredItems = dataArrayR
            //                .Where(item => item["Title"] != null &&
            //                               item["Title"].ToString().IndexOf(pesquisa, StringComparison.OrdinalIgnoreCase) >= 0)
            //                .ToList();

            //            foreach (var item in filteredItems)
            //            {
            //                //string endpointGameR = "https://retroachievements.org/API/API_GetGameExtended.php?i=" + Convert.ToInt32(item["ID"]) + "&z=PaulinoHearts&y=S7NrWqosUBkAa7v5GzYKiiaBDWJRyBOt";
            //                string endpointGameR = "https://retroachievements.org/API/API_GetGameExtended.php?i=" + Convert.ToInt32(item["ID"]) + "{Conexao.ConectarAPI(2)}";

            //                HttpResponseMessage responseGameR = await client.GetAsync(endpointGameR);
            //                string responseDataGameR = await responseGameR.Content.ReadAsStringAsync();
            //                JObject jsonObject = JObject.Parse(responseDataGameR);

            //                dto.listSearch.Add(new GamesSearch
            //                {
            //                    id = Convert.ToInt32(jsonObject["ID"]),
            //                    nome = Convert.ToString(jsonObject["Title"]),
            //                    imagem = Convert.ToString("https://media.retroachievements.org/" + jsonObject["ImageBoxArt"]),
            //                    console = SanitizeString((string)jsonObject["ConsoleName"]),
            //                    plataforma = 1
            //                });
            //            }
            //        }
            //    }
            //    catch (TaskCanceledException)
            //    {
            //        // Se a operação demorar mais de 10 segundos, cai aqui
            //    }
            //}
                return dto;
        }

        public async Task<JogosDTO> GameExtended(int idGame, int plataform)
        {
            JogosDTO dto = new JogosDTO();
            HttpClient client = new HttpClient();
            string endpoint;

            if (plataform == 0)
            {
                endpoint = $"https://store.steampowered.com/api/appdetails?appids={idGame}&l=portuguese";
                HttpResponseMessage response = await client.GetAsync(endpoint);
                string responseData = await response.Content.ReadAsStringAsync();
                JObject jsonObject = JObject.Parse(responseData);
                JObject appDetails = jsonObject[$"{idGame}"]?["data"] as JObject;

                dto.id = idGame;
                dto.nome = appDetails.Value<string>("name");
                dto.descricao = appDetails.Value<string>("short_description");
                dto.plataformaG = "Steam";
                dto.imagem = "https://cdn.cloudflare.steamstatic.com/steam/apps/" + idGame + "/hero_capsule.jpg";
                dto.imgHeader = appDetails.Value<string>("capsule_image");
                dto.developer = string.Join(", ", appDetails["developers"]?.ToObject<string[]>() ?? new string[0]);
                dto.publisher = string.Join(", ", appDetails["publishers"]?.ToObject<string[]>() ?? new string[0]);
                dto.banner = appDetails.Value<string>("background");
                //dto.dtLancamento = appDetails["release_date"]["date"].Value<DateTime>();
                dto.categoria = appDetails["genres"]?[0]?["description"]?.Value<string>();
                try
                {
                    dto.requisitos = appDetails["pc_requirements"]["recommended"].Value<string>().Replace("<strong>", "<strong style='color:white'>").Replace("<li>", "<li style='color:white'>");
                }
                catch
                {
                    dto.requisitos = appDetails["pc_requirements"]["minimum"].Value<string>().Replace("<strong>", "<strong style='color:white'>").Replace("<li>", "<li style='color:white'>");
                }
                try
                {
                    dto.preco = appDetails["price_overview"]["final_formatted"].Value<string>();
                }
                catch
                {
                    dto.preco = null;
                }
                dto.plataforma = 0;

                JArray screenshotsArray = appDetails["screenshots"] as JArray;
                if (screenshotsArray != null)
                {
                    dto.screenshots = screenshotsArray
                        .Select(ss => ss.Value<string>("path_thumbnail"))
                        .ToArray();
                }
                dto.about = Regex.Replace(appDetails.Value<string>("about_the_game"), "<img\\b[^>]*>", "", RegexOptions.IgnoreCase);
                var imgTags = Regex.Matches(appDetails.Value<string>("about_the_game"), "<img\\b[^>]*>");
                dto.imgGif = new string[imgTags.Count];
                for (int i = 0; i < imgTags.Count; i++)
                {
                    dto.imgGif[i] = imgTags[i].Value;
                }

                //Achievements
                var endpointAchiev = $"http://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v0002/?key={Conexao.ConectarAPI(1)}&appid={idGame}&l=english&format=json";
                HttpResponseMessage responseAchiev = await client.GetAsync(endpointAchiev);
                string responseeAchiev = await responseAchiev.Content.ReadAsStringAsync();
                JObject AchievListJson = JObject.Parse(responseeAchiev);
                JArray dataArray = (JArray)AchievListJson["game"]?["availableGameStats"]?["achievements"];
                dto.listAchiev = new List<AchievementsDTO>();

                if (dataArray != null)
                {
                    for (int i = 0; i <= dataArray.Count - 1; i++)
                    {
                        JObject item = (JObject)dataArray[i];

                        var achievementDto = new AchievementsDTO
                        {
                            nome = item["displayName"]?.ToString(),
                            descricao = item["description"]?.ToString(),
                            iconLock = item["icon"]?.ToString(),
                        };
                        dto.listAchiev.Add(achievementDto);
                    }
                }
            }
            else
            {
                dto.screenshots = new string[2];
                //endpoint = $"https://retroachievements.org/API/API_GetGameExtended.php?i={idGame}&z=PaulinoHearts&y=S7NrWqosUBkAa7v5GzYKiiaBDWJRyBOt";
                endpoint = $"https://retroachievements.org/API/API_GetGameExtended.php?i={idGame}&{Conexao.ConectarAPI(2)}";
                HttpResponseMessage response = await client.GetAsync(endpoint);
                string responseData = await response.Content.ReadAsStringAsync();
                JObject jsonObject = JObject.Parse(responseData);

                dto.id = idGame;
                dto.nome = Convert.ToString(jsonObject["Title"]);
                dto.plataformaG = SanitizeString((string)jsonObject["ConsoleName"]);
                //dto.dtLancamento = Convert.ToDateTime(jsonObject["Released"]);
                dto.imagem = Convert.ToString("https://media.retroachievements.org/" + jsonObject["ImageBoxArt"]);
                dto.imgHeader = Convert.ToString("https://media.retroachievements.org/" + jsonObject["ImageBoxArt"]);
                dto.categoria = Convert.ToString(jsonObject["Genre"]);
                dto.developer = Convert.ToString(jsonObject["Developer"]);
                dto.publisher = Convert.ToString(jsonObject["Publisher"]);
                dto.plataforma = 1;
                dto.screenshots[0] = Convert.ToString("https://media.retroachievements.org/" + jsonObject["ImageTitle"]);
                dto.screenshots[1] = Convert.ToString("https://media.retroachievements.org/" + jsonObject["ImageIngame"]);

                //Achievements
                var endpointAchiev = $"https://retroachievements.org/API/API_GetGameInfoAndUserProgress.php?{Conexao.ConectarAPI(2)}&g={idGame}";
                HttpResponseMessage responseAchiev = await client.GetAsync(endpointAchiev);

                string responseeAchiev = await responseAchiev.Content.ReadAsStringAsync();
                JObject AchievListJson = JObject.Parse(responseeAchiev);
                var achievementsObject = AchievListJson["Achievements"] as JObject;
                JArray dataArray = new JArray(achievementsObject.Properties().Select(p => p.Value));

                if (dataArray != null)
                {
                    dto.listAchiev = new List<AchievementsDTO>();

                    // Processa cada achievement no array
                    foreach (var achievement in dataArray)
                    {
                        var item = (JObject)achievement;

                        var achievementDto = new AchievementsDTO
                        {
                            nome = item["Title"]?.ToString(),
                            descricao = item["Description"]?.ToString(),
                            iconLock = "https://media.retroachievements.org/Badge/" + item["BadgeName"]?.ToString() + ".png",
                        };

                        dto.listAchiev.Add(achievementDto);
                    }
                }
            }
            return dto;
        }
        #endregion

        #region music
            
        public async Task<GamesDTO> ApiLofi()
        {
            GamesDTO dto = new GamesDTO();
            HttpClient client = new HttpClient();
            var endpoint = $"https://api.jamendo.com/v3.0/tracks?client_id=68b8a566&format=json&tags=lofi&limit=5";
            HttpResponseMessage response = await client.GetAsync(endpoint);

            string responseMusic = await response.Content.ReadAsStringAsync();
            JObject MusicListJson = JObject.Parse(responseMusic);

            // Acesse "results" como um JArray, já que é um array de objetos
            JArray resultsArray = (JArray)MusicListJson["results"];

            if (resultsArray != null)
            {
                dto.listMusicas = new List<MusicasDTO>();

                // Processa cada achievement no array
                foreach (var music in resultsArray)
                {
                    var item = (JObject)music;

                    var MusicasDTO = new MusicasDTO
                    {
                        nome = item["name"]?.ToString(),
                        sound = item["audio"]?.ToString(),
                    };

                    dto.listMusicas.Add(MusicasDTO);
                }
            }
            return dto;

        }
        #endregion
        #endregion
    }
}
