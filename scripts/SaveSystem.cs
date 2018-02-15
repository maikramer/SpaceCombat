using Godot;
using Newtonsoft.Json;

namespace MkGames {
    public class SaveSystem<T> : Node {
        private T m_object;
        private string m_path;
        private int m_time;
        private int updatePeriod = 1000; //Em ms
        private int m_lastMod;
        public SaveSystem (string fileName, ref T obj, Node attachToNode) {
            if (!System.Text.RegularExpressions.Regex.IsMatch (fileName, @"^[a-zA-Z0-9_]+$")) {
                GD.Print ($"O nome de arquivo {fileName} e invalido, somente letras, numeros e _");
                return;
            }

            m_object = obj;
            attachToNode.AddChild (this);

            using (File fl = new File ()) {

                m_path = "user://" + fileName + ".json";
                if (fl.FileExists (m_path)) {
                    fl.Open (m_path, (int) File.ModeFlags.Read);
                    m_object = JsonConvert.DeserializeObject<T> (fl.GetAsText ());
                } else {
                    fl.Open (m_path, (int) File.ModeFlags.Write);
                    fl.StoreString (JsonConvert.SerializeObject (m_object));
                }

                m_lastMod = fl.GetModifiedTime (m_path);
            }
        }

        public override void _Process (float delta) {
            m_time += (int) (delta * 1000);
            if (m_time % updatePeriod == 0) {
                using (File fl = new File ()) {
                    var lastMod = fl.GetModifiedTime (m_path);
                    if (lastMod > m_lastMod) {
                        fl.Open (m_path, (int) File.ModeFlags.Read);
                        m_object = JsonConvert.DeserializeObject<T> (fl.GetAsText ());
                    }
                }
            }
        }

    }//Class
}//Namespace