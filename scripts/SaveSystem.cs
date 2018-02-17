using Godot;
using Newtonsoft.Json;

namespace MkGames {
    public class SaveSystem<T> : Node {
        private const int UPDATE_PERIOD = 200;
        
        private T m_object;
        private string m_path;
        private long m_time;
        private long m_lastUpdate;
        private int m_updatePeriod = UPDATE_PERIOD; //Em ms
        private int m_lastMod;
        public SaveSystem (string fileName, ref T obj, Node attachToNode) {
            if (!System.Text.RegularExpressions.Regex.IsMatch (fileName, @"^[a-zA-Z0-9_]+$")) {
                GD.Print ($"O nome de arquivo {fileName} e invalido, somente letras, numeros e _");
                return;
            }

            m_object = obj;

            using (File fl = new File ()) {

                m_path = "user://" + fileName + ".json";
                if (fl.FileExists (m_path)) {
                    fl.Open (m_path, (int) File.ModeFlags.Read);
                    CopyAll (JsonConvert.DeserializeObject<T> (fl.GetAsText ()), m_object);
                } else {
                    fl.Open (m_path, (int) File.ModeFlags.Write);
                    fl.StoreString (JsonConvert.SerializeObject (m_object));
                }
            }
            Name = "SaveSystem " + typeof (T);
            attachToNode.AddChild (this);

        }

        public override void _Ready () {
            using (File fl = new File ()) {
                m_lastMod = fl.GetModifiedTime (m_path);
            }
        }

        public override void _Process (float delta) {
            if (delta * 1000 > 1)
                m_time = (int) (m_time + delta * 1000);
            else
                m_time++;

            if (m_time - m_lastUpdate > m_updatePeriod) {
                using (File fl = new File ()) {
                    var mod = fl.GetModifiedTime (m_path);
                    if (mod > m_lastMod) {
                        fl.Open (m_path, (int) File.ModeFlags.Read);
                        CopyAll (JsonConvert.DeserializeObject<T> (fl.GetAsText ()), m_object);
                        m_lastMod = mod;
                    }
                }
                m_lastUpdate = m_time;
            }
        }

        public void CopyAll (T source, T target) {
            var type = typeof (T);
            foreach (var sourceProperty in type.GetProperties ()) {
                var targetProperty = type.GetProperty (sourceProperty.Name);
                targetProperty.SetValue (target, sourceProperty.GetValue (source, null), null);
            }
            foreach (var sourceField in type.GetFields ()) {
                var targetField = type.GetField (sourceField.Name);
                targetField.SetValue (target, sourceField.GetValue (source));
            }
        }

    } //Class
} //Namespace