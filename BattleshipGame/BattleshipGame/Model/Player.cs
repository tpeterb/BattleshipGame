﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipGame.Model
{
    public class Player
    {
        public PlayerType TypeOfPlayer { get; set; }

        public string PlayerName { get; set; }

        public Player(PlayerType playerType, string playerName)
        {
            TypeOfPlayer = playerType;
            if (IsPlayerNameValid(playerName))
            {
                PlayerName = playerName;
            } else
            {
                throw new ArgumentException("The player's name is invalid, it cannot be empty, consist of whitespaces, or contain special characters!");
            }
        }

        private bool IsPlayerNameValid(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                return false;
            }
            char[] specialCharacters = { '!', '?', '_', '-', '.', ':', '#', '(', ')', '{', '}', '[', ']' };
            foreach (var character in specialCharacters)
            {
                if (playerName.Contains(character))
                {
                    return false;
                }
            }
            return true;
        }
    }
}