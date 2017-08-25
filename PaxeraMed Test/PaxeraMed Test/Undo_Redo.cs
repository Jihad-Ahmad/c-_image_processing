using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PaxeraMed_Test
{
    class UndoRedoFeature
    {

        private Bitmap[] UndoRedo;
        private int lastin;
        //private int firstout;
        private bool full;
        private bool cyclic;

        public UndoRedoFeature()
        {
            this.UndoRedo = new Bitmap[100];
            for(int i=0;i<99;i++)
            {
                UndoRedo[i] = null;
            } 
            full = false;
            cyclic = false;

        }

        public void AddToArray(Bitmap b)
        {
            if (full && (lastin == 99))
            {
                lastin = 0;
                this.UndoRedo[lastin] = new Bitmap((Image)b);
                cyclic = true;
            }
            else
            {
                if (lastin == 98) { full = true; }
                this.UndoRedo[++lastin] = new Bitmap((Image)b);
            }
        }

        public Bitmap Undo()
        {
            if (lastin < 0) { return null; }
            if (cyclic && lastin == 0) { lastin = 99; return this.UndoRedo[(lastin)]; }
            Bitmap b = this.UndoRedo[(lastin)];
            lastin--;
            return b;
        }

        public Bitmap Redo()
        {
            if (lastin == 99 && cyclic)
            {
                lastin = 0;
                return this.UndoRedo[lastin];
            }
            if (full && !cyclic && lastin == 99){ return null;}
            if (this.UndoRedo[(lastin + 1)] == null) { return null; }
            
            
            lastin++;
            return this.UndoRedo[lastin];
            
        }
    }
}