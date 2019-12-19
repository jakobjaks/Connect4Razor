using System.ComponentModel;

namespace Domain
{
    public enum GameMode
    {
        [Description("AI First")]
        AI_FIRST,
        
        [Description("Human First")]
        HUMAN_FIRST,
        
        [Description("Human vs Human")]
        HUMAN_VS_HUMAN,
    }
}