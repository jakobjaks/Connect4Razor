using System;

namespace BLL
{
    public class GameException : Exception
    {
        public GameException(GameExceptionType e)
        {
            switch (e)
            {
                case GameExceptionType.COLUMN_FULL:
                    throw new GameException(GameExceptionType.COLUMN_FULL);
            }
        } 
    }

    public enum GameExceptionType
    {
        COLUMN_FULL
    }
}