using GameFramework.Core;
using GameFramework.Core.GameFramework.Manager;
using GameFramework.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Game
{
    public class GameLobbyManager : Singleton<GameLobbyManager>
    {
        private List<LobbyPlayerData> _lobbyPLayerData = new List<LobbyPlayerData>();

        private LobbyPlayerData localLobbyPlayerData;
        private void OnEnable()
        {
            LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }



        private void OnDisable()
        {
            LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        }
        // M�todo para obtener el c�digo del lobby desde el LobbyManager
        public string GetLobbyCode()
        {
            return LobbyManager.Instance.GetLobbyCode();
        }

        // M�todo para crear un lobby de juego de forma as�ncrona
        public async Task<bool> CreateLobby()
        {
            // Se crea un objeto LobbyPlayerData que representa los datos del jugador anfitri�n.
            LobbyPlayerData playerData = new LobbyPlayerData();
            playerData.Initialize(AuthenticationService.Instance.PlayerId, "HostPlayer");

            // Se llama al m�todo CreateLobby del LobbyManager para crear el lobby con los siguientes par�metros:
            // - Tiempo l�mite del lobby: 30 segundos
            // - Se permite iniciar el juego autom�ticamente cuando todos los jugadores est�n listos (true)
            // - Los datos del jugador anfitri�n se serializan y se env�an al lobby para que los dem�s jugadores puedan acceder a ellos.
            bool succeeded = await LobbyManager.Instance.CreateLobby(30, true, playerData.Serialize());

            // Se devuelve el resultado de la creaci�n del lobby.
            return succeeded;
        }

        // M�todo para unirse a un lobby existente de forma as�ncrona
        public async Task<bool> JoinLobby(string code)
        {

            LobbyPlayerData playerData = new LobbyPlayerData();
            playerData.Initialize(AuthenticationService.Instance.PlayerId, "JoinPlayer");
            // Se llama al m�todo JoinLobby del LobbyManager para unirse al lobby con el c�digo proporcionado y los datos del jugador.
            bool succeeded = await LobbyManager.Instance.JoinLobby(code, playerData.Serialize());

            // Se devuelve el resultado de unirse al lobby.
            return succeeded;
        }

        private void OnLobbyUpdated(Lobby lobby)
        {
            List<Dictionary<string, PlayerDataObject>> playerData = LobbyManager.Instance.GetPlayersData();
            _lobbyPLayerData.Clear();

            foreach (Dictionary<string, PlayerDataObject> data in playerData)
            {
                LobbyPlayerData lobbyPlayerData = new LobbyPlayerData();
                lobbyPlayerData.Initialize(data);


                if (lobbyPlayerData.Id == AuthenticationService.Instance.PlayerId)
                {
                    localLobbyPlayerData = lobbyPlayerData;
                }

                _lobbyPLayerData.Add(lobbyPlayerData);
            }

            Events.LobbyEvents.OnLobbyUpdated?.Invoke();
        }

        public List<LobbyPlayerData> GetPlayers()
        {
            return _lobbyPLayerData;
        }
    }

}