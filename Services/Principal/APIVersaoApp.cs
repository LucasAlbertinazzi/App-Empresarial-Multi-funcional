﻿using AppMarciusMagazine.Classes.API.Principal;
using AppMarciusMagazine.Classes.Globais;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AppMarciusMagazine.Services.Principal
{
    public class APIVersaoApp
    {
        #region 1- LOG
        APIErroLog error = new();

        private async Task MetodoErroLog(Exception ex)
        {
            var erroLog = new ErrorLogClass
            {
                Erro = ex.Message, // Obtém a mensagem de erro
                Metodo = ex.TargetSite.Name, // Obtém o nome do método que gerou o erro
                Dispositivo = DeviceInfo.Model, // Obtém o nome do dispositivo em execução
                Versao = DeviceInfo.Version.ToString(), // Obtém a versão do dispostivo
                Plataforma = DeviceInfo.Platform.ToString(), // Obtém o sistema operacional do dispostivo
                TelaClasse = GetType().FullName, // Obtém o nome da tela/classe
                Data = DateTime.Now,
            };

            await error.LogErro(erroLog);
        }
        #endregion

        #region 2- API
        private HttpClient _httpClient;

        public APIVersaoApp()
        {
            _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 10) };
        }

        private string RemoveLast(string versao)
        {
            try
            {
                // Verificar se a string termina com ".0.0"
                if (versao.EndsWith(".0.0"))
                {
                    // Encontrar a posição da última ocorrência de ".0.0"
                    int posicaoRemover = versao.LastIndexOf(".0.0");

                    // Verificar se a posição foi encontrada
                    if (posicaoRemover >= 0)
                    {
                        // Remover os últimos 4 caracteres
                        return versao.Substring(0, posicaoRemover);
                    }
                }

                return versao;
            }
            catch (Exception ex)
            {
                MetodoErroLog(ex);
                return null;
            }
        }

        private async Task<bool> VerificarVersaoInstalada()
        {
            try
            {
                string versaoInstalada = AppInfo.Version.ToString();

                string url = InfoGlobal.apiApp + "/AppVersion/verifica-versao";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var ultimaLinha = response.Content.ReadAsAsync<VersaoAppClass>();

                InfoGlobal.LastVersao = RemoveLast(ultimaLinha.Result.Versao);

                // Comparar a versão instalada com a versão obtida do endpoint
                if (versaoInstalada == ultimaLinha.Result.Versao)
                {
                    return true; // Versão instalada está atualizada
                }

                return false; // Versão instalada não está atualizada
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return false;
            }
        }

        public async Task<bool> InserirVersaoNaAPI(VersaoAppClass versionInfo)
        {
            try
            {
                // Serialize o objeto versionInfo para JSON
                string json = JsonConvert.SerializeObject(versionInfo);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Faça uma requisição POST para a rota "inserir-versao" na sua API

                string url = InfoGlobal.apiApp + "/AppVersion/inserir-versao";
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                // Verifique se a resposta foi bem-sucedida
                response.EnsureSuccessStatusCode();

                // Verifique o status code da resposta para determinar se a inserção foi bem-sucedida
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return false;
            }
        }

        public async Task SalvaVersao()
        {
            try
            {
                string versao = RemoveLast(typeof(App).Assembly.GetName().Version.ToString());

                if (ComparaVersao(InfoGlobal.LastVersao, versao))
                {
                    await Application.Current.MainPage.DisplayAlert("A ultima versão registrada é:" + InfoGlobal.LastVersao + "", "Atualize a versão do projeto manualmente para uma(1) versão acima!", "OK");
                }
                else
                {
                    var novaVersao = new VersaoAppClass
                    {
                        Versao = versao,
                        Data = DateTime.Now
                    };

                    if (!await InserirVersaoNaAPI(novaVersao))
                    {
                        await Application.Current.MainPage.DisplayAlert("Erro", "Erro ao salvar nova versão do APP na Base da dados, verifique o erro!", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return;
            }
        }

        private bool ComparaVersao(string banco, string apk)
        {
            try
            {
                if (!string.IsNullOrEmpty(banco) && !string.IsNullOrEmpty(apk))
                {
                    string[] numbers1 = banco.Split('.');
                    string[] numbers2 = apk.Split('.');

                    bool version1IsGreater = false;

                    for (int i = 0; i < numbers1.Length; i++)
                    {
                        int num1 = int.Parse(numbers1[i]);
                        int num2 = int.Parse(numbers2[i]);

                        if (num1 > num2)
                        {
                            version1IsGreater = true;
                            break;
                        }
                        else if (num1 < num2)
                        {
                            break;
                        }
                    }

                    if (version1IsGreater)
                    {
                        //Versão do BANCO é maior que a versão do APK
                        return true;
                    }
                    else if (banco == apk)
                    {
                        //Versão do BANCO é igual a versão do APK
                        return false;
                    }
                    else
                    {
                        //Versão do APK é maior que a versão do BANCO
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MetodoErroLog(ex);
                return false;
            }
        }

        public async Task<bool> VerificaIp()
        {
            if (GetIPAddress().Contains("10.10") || GetIPAddress().Contains("192.168"))
            {
                #region URL - Produção
                InfoGlobal.apiApp = "http://192.168.10.3:6162/api";
                InfoGlobal.apiCobranca = "http://192.168.10.3:6163/api";
                #endregion

                #region URL - Desenvolvimento
                if (Debugger.IsAttached)
                {
                    InfoGlobal.apiApp = "http://192.168.10.94:5000/api";
                    InfoGlobal.apiCobranca = "http://192.168.10.94:6000/api";
                }
                #endregion
            }

            return await VerificarVersaoInstalada();
        }

        private string GetIPAddress()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                // Nenhuma conexão de rede disponível
                return null;
            }

            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (var address in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            // Endereço IPv4 encontrado
                            return address.Address.ToString();
                        }
                    }
                }
            }

            return null; // Nenhum endereço IPv4 encontrado
        }
        #endregion
    }
}
