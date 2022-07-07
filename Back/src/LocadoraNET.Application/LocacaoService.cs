using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using LocadoraNET.Application.Contracts;
using LocadoraNET.Application.Dtos;
using LocadoraNET.Domain;
using LocadoraNET.Persistence.Contracts;

namespace LocadoraNET.Application
{
    public class LocacaoService : ILocacaoService
    {
        private readonly IGeneralPersist _generalPersist;
        private readonly ILocacaoPersist _locacaoPersist;
        private readonly IMapper _mapper;

        public LocacaoService(IGeneralPersist generalPersist, ILocacaoPersist locacaoPersist, IMapper mapper)
        {
            _generalPersist = generalPersist;
            _locacaoPersist = locacaoPersist;
            _mapper = mapper;
        }
        public async Task<LocacaoDto> AddLocacao(LocacaoDto model)
        {
            try
            {
                var locacao = _mapper.Map<Locacao>(model);
                _generalPersist.Add<Locacao>(locacao);

                if(!await _generalPersist.SaveChangesAsync())
                    return null;
                    
                var result = await _locacaoPersist.GetLocacaoById(locacao.Id);
                return _mapper.Map<LocacaoDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LocacaoDto> UpdateLocacao(int locacaoId, LocacaoDto model)
        {
            try
            {
                var locacao = await _locacaoPersist.GetLocacaoById(locacaoId);
                if(locacao == null) return null;

                model.Id = locacao.Id;
                _mapper.Map(model, locacao);
                _generalPersist.Update<Locacao>(locacao);

                if(!await _generalPersist.SaveChangesAsync())
                    return null;
                    
                var result = await _locacaoPersist.GetLocacaoById(locacao.Id);
                return _mapper.Map<LocacaoDto>(result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteLocacao(int locacaoId)
        {
            try
            {
                var locacao = await _locacaoPersist.GetLocacaoById(locacaoId);
                
                if(locacao == null) throw new Exception("'Locacao' for delete not found!");

                _generalPersist.Delete<Locacao>(locacao);

                return await _generalPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LocacaoDto[]> GetAllLocacoes()
        {
            try
            {
                var locacao = await _locacaoPersist.GetAllLocacoes();
                if(locacao == null) return null;

                return _mapper.Map<LocacaoDto[]>(locacao);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LocacaoDto> GetLocacaoById(int locacaoId)
        {
            try
            {
                var locacao = await _locacaoPersist.GetLocacaoById(locacaoId);
                if(locacao == null) return null;

                return _mapper.Map<LocacaoDto>(locacao);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LocacaoDto[]> GetAllLocacoesByClienteId(int clienteId)
        {
            try
            {
                var locacao = await _locacaoPersist.GetAllLocacoesByClienteId(clienteId);
                if(locacao == null) return null;

                return _mapper.Map<LocacaoDto[]>(locacao);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LocacaoDto[]> GetAllLocacoesByFilmeId(int filmeId)
        {
            try
            {
                var locacao = await _locacaoPersist.GetAllLocacoesByFilmeId(filmeId);
                if(locacao == null) return null;

                return _mapper.Map<LocacaoDto[]>(locacao);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public byte[] GerarPlanilha()
        {
            var relatorio = new RelatorioDto() {
                        ClienteAtraso = _locacaoPersist.SqlRaw("SELECT c.nome FROM cliente c INNER JOIN locacao l ON l.ClienteId = c.Id	 WHERE l.DataDevolucao < CURDATE()"),
                        FilmesNaoAlugados = _locacaoPersist.SqlRaw("SELECT f.Titulo FROM Filme f LEFT JOIN locacao l ON l.FilmeId = f.Id WHERE l.FilmeId IS null"), 
                        Top3FilmesSemana = _locacaoPersist.SqlRaw("SELECT f.Titulo FROM filme f INNER JOIN locacao l ON f.Id = l.filmeId WHERE l.dataLocacao BETWEEN DATE(CURDATE() - INTERVAL 7 DAY) AND DATE(CURDATE()) GROUP BY l.filmeId order by COUNT(l.filmeId) ASC LIMIT 3"),
                        Top5Filmesano = _locacaoPersist.SqlRaw("SELECT f.Titulo FROM filme f INNER JOIN locacao l ON f.Id = l.filmeId WHERE YEAR(l.dataLocacao) = YEAR(CURDATE()) GROUP BY l.filmeId order by COUNT(l.filmeId) DESC LIMIT 5"),
                        SegundoCliente = _locacaoPersist.SqlRaw("SELECT c.nome FROM cliente c INNER JOIN locacao l ON l.ClienteId = c.Id GROUP BY l.ClienteId order by COUNT(l.ClienteId) DESC LIMIT 1 OFFSET 1")[0]
            };

            MemoryStream mem = new MemoryStream();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.
                Create(mem, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
                                                    AppendChild<Sheets>(new Sheets());

                Sheet sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "mySheet"
                };

                sheets.Append(sheet);
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                UInt32 rowIndex = 1;
                var clienteAtraso = new Cell();

                foreach (var item in relatorio.ClienteAtraso)
                {
                    var row = new Row() { RowIndex = rowIndex };
                    if(rowIndex == 1){

                        clienteAtraso = new Cell() { CellReference = "A" + (rowIndex + 1) };
                        clienteAtraso.CellValue = new CellValue("Clientes em atraso");
                        clienteAtraso.DataType = CellValues.String;

                        row.AppendChild(clienteAtraso);

                        sheetData.AppendChild(row);
                        rowIndex++;
                        continue;
                    }
                    clienteAtraso = new Cell() { CellReference = "A" + (rowIndex + 1) };
                    clienteAtraso.CellValue = new CellValue(item);
                    clienteAtraso.DataType = new EnumValue<CellValues>(CellValues.String);

                    row.AppendChild(clienteAtraso);
                    sheetData.AppendChild(row);
                    rowIndex++;
                }

                rowIndex = 1;
                var filmesNaoAlugados = new Cell();
                foreach (var item in relatorio.FilmesNaoAlugados)
                {
                    var row = new Row() { RowIndex = rowIndex };
                    if(rowIndex == 1){

                        filmesNaoAlugados = new Cell() { CellReference = "B" + (rowIndex + 1) };
                        filmesNaoAlugados.CellValue = new CellValue("Filmes nunca alugados");
                        filmesNaoAlugados.DataType = CellValues.String;

                        row.AppendChild(filmesNaoAlugados);
                        sheetData.AppendChild(row);
                        rowIndex++;
                        continue;
                    }
                    filmesNaoAlugados = new Cell() { CellReference = "B" + (rowIndex + 1) };
                    filmesNaoAlugados.CellValue = new CellValue(item);
                    filmesNaoAlugados.DataType = new EnumValue<CellValues>(CellValues.String);

                    row.AppendChild(filmesNaoAlugados);
                    sheetData.AppendChild(row);
                    rowIndex++;
                }

                rowIndex = 1;
                var top5Filmes = new Cell();
                foreach (var item in relatorio.Top5Filmesano)
                {
                    var row = new Row() { RowIndex = rowIndex };
                    if(rowIndex == 1){

                        top5Filmes = new Cell() { CellReference = "C" + (rowIndex + 1) };
                        top5Filmes.CellValue = new CellValue("Top 5 filmes mais alugados no ano");
                        top5Filmes.DataType = CellValues.String;

                        row.AppendChild(top5Filmes);
                        sheetData.AppendChild(row);
                        rowIndex++;
                        continue;
                    }
                    top5Filmes = new Cell() { CellReference = "C" + (rowIndex + 1) };
                    top5Filmes.CellValue = new CellValue(item);
                    top5Filmes.DataType = new EnumValue<CellValues>(CellValues.String);

                    row.AppendChild(top5Filmes);
                    sheetData.AppendChild(row);
                    rowIndex++;
                }

                rowIndex = 1;
                var top3Filmes = new Cell();
                foreach (var item in relatorio.Top3FilmesSemana)
                {
                    var row = new Row() { RowIndex = rowIndex };
                    if(rowIndex == 1){

                        top3Filmes = new Cell() { CellReference = "D" + (rowIndex + 1) };
                        top3Filmes.CellValue = new CellValue("Top 3 filmes menos alugados na semana");
                        top3Filmes.DataType = CellValues.String;

                        row.AppendChild(top3Filmes);
                        sheetData.AppendChild(row);
                        rowIndex++;
                        continue;
                    }
                    top3Filmes = new Cell() { CellReference = "D" + (rowIndex + 1) };
                    top3Filmes.CellValue = new CellValue(item);
                    top3Filmes.DataType = new EnumValue<CellValues>(CellValues.String);

                    row.AppendChild(top3Filmes);
                    sheetData.AppendChild(row);
                    rowIndex++;
                }

                rowIndex = 1;
                var segundoCliente = new Cell();

                var rowAux = new Row() { RowIndex = rowIndex };

                segundoCliente = new Cell() { CellReference = "E" + (rowIndex + 1) };
                segundoCliente.CellValue = new CellValue("Segundo cliente com mais loca��es");
                segundoCliente.DataType = CellValues.String;

                rowAux.AppendChild(segundoCliente);
                sheetData.AppendChild(rowAux);

                rowIndex++;
                rowAux = new Row() { RowIndex = rowIndex };

                segundoCliente = new Cell() { CellReference = "E" + (rowIndex + 1) };
                segundoCliente.CellValue = new CellValue(relatorio.SegundoCliente);
                segundoCliente.DataType = CellValues.String;

                rowAux.AppendChild(segundoCliente);
                sheetData.AppendChild(rowAux);

                workbookpart.Workbook.Save();
                spreadsheetDocument.Close();
                return mem.ToArray();
            }
        }
    }
}