using System;

namespace Alkawa.Core
{
    [Flags]
    public enum EUserRole
    {
        Prog = 1 << 1,
        GPP = 1 << 2,
        Tool = 1 << 3,
        GD = 1 << 4,
        LD = 1 << 5,
        FX = 1 << 6,
        Sound = 1 << 7,
        Animations = 1 << 8,
        Art = 1 << 9,
        QA = 1 << 10,
        CharacterArtist = 1 << 11,
        Light = 1 << 12,
        Cine = 1 <<13,
        
        GFX_ROLES = Art | CharacterArtist | Cine | Light | FX | Prog,
        GAMEPLAY_ROLES = Animations| GD | GPP | LD | Sound | Prog,
        

        All = int.MaxValue
    }
    
}
