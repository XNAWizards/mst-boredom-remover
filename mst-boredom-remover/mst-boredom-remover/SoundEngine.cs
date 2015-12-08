using System;
using System.Collections.Generic;
using System.Linq;
using mst_boredom_remover.engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace mst_boredom_remover
{
    class SoundEngine
    {
        public static Song backgroundMusic;
        
        public static void PlayBGM()
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
        }
        public static void StopBGM()
        {
            MediaPlayer.Stop();
        }
    }
}