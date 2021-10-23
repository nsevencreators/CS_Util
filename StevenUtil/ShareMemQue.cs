using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.MemoryMappedFiles;
using System.IO;

using StevenUtility;

namespace StevenUtility
{
    public class ShareMemQue
    {
        MemoryMappedFile _f;
        MemoryMappedFile _f_idx;

        string f_name;
        UInt32 _size;
        public bool Overflow = false;

        public UInt32 r_idx = 0;
        public UInt32 w_idx = 0;

        const int W_INDEX_POS = 0;
        const int R_INDEX_POS = 4;
        
        public UInt32 count
        {
            get
            {
                if (w_idx == r_idx) return 0;
                else if (w_idx > r_idx) return (w_idx - r_idx);
                else return (_size - (r_idx - w_idx));
            }
        }
                
        public ShareMemQue(string name, UInt32 size)
        {
            _f = MemoryMappedFile.CreateOrOpen(name, size);
            _f_idx = MemoryMappedFile.CreateOrOpen(name + "_idx", 10);

            f_name = name;
            _size = size;

            r_idx = 0;
            w_idx = 0;
        }

        void reload_index()
        {
            using (MemoryMappedViewAccessor acc_idx = _f_idx.CreateViewAccessor())
            {
                // update w_idx
                acc_idx.Read(W_INDEX_POS, out w_idx);
                acc_idx.Read(R_INDEX_POS, out r_idx);
            }
        }

        void update_index()
        {
            using (MemoryMappedViewAccessor acc_idx = _f_idx.CreateViewAccessor())
            {
                acc_idx.Write(W_INDEX_POS, w_idx);
                acc_idx.Write(R_INDEX_POS, r_idx);
            }
        }


        public void Write(string str)
        {
            reload_index();

            using (MemoryMappedViewAccessor acc = _f.CreateViewAccessor())
            {
                byte[] arr = Encoding.UTF8.GetBytes(str);
                for (int i = 0; i < str.Length; i++)
                {
                    if (count == (_size-1))
                    {
                        Overflow = true;
                        r_idx++;
                        if (r_idx >= _size) r_idx = 0;
                    }

                    acc.WriteArray(w_idx++, arr, i, 1);
                    if (w_idx >= _size) w_idx = 0;
                }
            }
  
            update_index();
        }

        public List<byte> GetArray()
        {
            List<byte> data = new List<byte>();

            reload_index();

            using (MemoryMappedViewAccessor acc = _f.CreateViewAccessor())
            {

                byte b;
                for(int i = 0; i < _size; i++)
                {
                    acc.Read(i, out b);
                    data.Add(b);
                }
                return data;
            }
        }

        public List<byte> Read(UInt32 num)
        {
            List<byte> data = new List<byte>();

            reload_index();

            using (MemoryMappedViewAccessor acc = _f.CreateViewAccessor())
            {

                byte b;
                for (int i = 0; i < num; i++)
                {
                    //r_idx = r_pos;
                    if (count == 0) break;
                    acc.Read(r_idx++, out b);
                    data.Add(b);
                    if (r_idx >= _size) r_idx = 0;
                }
            }

            update_index();

            return data;
        }

        
    }
}
