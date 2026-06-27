using UnityEngine;



namespace HexaChessMirrorXR
{
    public class PlayerChessXrTagMono_RightHand : MonoBehaviour
    {
        public static Transform _in_scene_tag;
        void Awake() => _in_scene_tag = this.transform;
   
    }
}