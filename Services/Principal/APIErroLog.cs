﻿using AppEmpresarialMultFuncional.Classes.API.Principal;
using AppEmpresarialMultFuncional.Classes.Globais;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEmpresarialMultFuncional.Services.Principal
{
    public class APIErroLog
    {
        private HttpClient _httpClient;

        public APIErroLog()
        {
            _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
        }

        public async Task<bool> LogErro(ErrorLogClass erro)
        {
            try
            {
                // Serialize o objeto versionInfo para JSON
                string json = JsonConvert.SerializeObject(erro);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Faça uma requisição POST para a rota "inserir-versao" na sua API

                string url = InfoGlobal.apiApp + "/Log/erro";
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                // Verifique se a resposta foi bem-sucedida
                response.EnsureSuccessStatusCode();

                await Application.Current.MainPage.DisplayAlert("Erro", "Ocorreu um erro ao executar está ação, por favor, tente novamente mais tarde!", "OK");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
