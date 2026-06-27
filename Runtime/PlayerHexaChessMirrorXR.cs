using UnityEngine;
using Mirror;



namespace HexaChessMirrorXR
{
    public class PlayerHexaChessMirrorXR : NetworkBehaviour
    {
        [SyncVar]
        [SerializeField]
        Vector3 _localHeadFromBoardPosition;
        [SyncVar]
        [SerializeField]
        Quaternion _localHeadFromBoardRotation;


        [SyncVar]
        [SerializeField]
        Vector3 _localLeftHandFromBoardPosition;
        
        [SyncVar]
        [SerializeField]
        Quaternion _localLeftHandFromBoardRotation;


        [SyncVar]
        [SerializeField]
        Vector3 _localRigthHandFromBoardPosition;

        [SyncVar]
        [SerializeField]
        Quaternion _localRigthHandFromBoardRotation;


        [SerializeField]
        Transform _head;

        [SerializeField]
        Transform _leftHand;
        
        [SerializeField]
        Transform _rightHand;


        [SerializeField] bool m_useFixedUpdateToRefresh= true;



    public void SendLocalComputedInformationToMirrorServer()
    {
            if (base.isLocalPlayer)
            {
                Transform chessBoard = PlayerChessXrTagMono_ChessBoardCenter._in_scene_tag;
                if (chessBoard == null)
                {
                    Debug.LogError("PlayerChessXrTagMono_ChessBoardCenter._in_scene_tag is null");
                    return;
                }
                if (PlayerChessXrTagMono_Head._in_scene_tag != null
                    && PlayerChessXrTagMono_LeftHand._in_scene_tag != null
                    && PlayerChessXrTagMono_RightHand._in_scene_tag != null
                    ) {
                    GetLocal(PlayerChessXrTagMono_Head._in_scene_tag, chessBoard, out Vector3 localHeadFromBoardPosition, out Quaternion localHeadFromBoardRotation);
                    GetLocal(PlayerChessXrTagMono_LeftHand._in_scene_tag, chessBoard, out Vector3 localLeftHandFromBoardPosition, out Quaternion localLeftHandFromBoardRotation);
                    GetLocal(PlayerChessXrTagMono_RightHand._in_scene_tag, chessBoard, out Vector3 localRigthHandFromBoardPosition, out Quaternion localRigthHandFromBoardRotation);
                    CmdSendLocalComputedInformationToMirrorServer(
                        localHeadFromBoardPosition,
                        localLeftHandFromBoardPosition,
                        localRigthHandFromBoardPosition,
                        localHeadFromBoardRotation,
                        localLeftHandFromBoardRotation,
                        localRigthHandFromBoardRotation);
                }
            }
        }

        [Command]
        private void CmdSendLocalComputedInformationToMirrorServer(Vector3 localHeadFromBoardPosition, Vector3 localLeftHandFromBoardPosition, Vector3 localRigthHandFromBoardPosition, Quaternion localHeadFromBoardRotation, Quaternion localLeftHandFromBoardRotation, Quaternion localRigthHandFromBoardRotation)
        {
            // We are on the server now, so we can update the SyncVars and they will be automatically synchronized to all clients
            _localHeadFromBoardPosition = localHeadFromBoardPosition;
            _localLeftHandFromBoardPosition = localLeftHandFromBoardPosition;
            _localRigthHandFromBoardPosition = localRigthHandFromBoardPosition;
            _localHeadFromBoardRotation = localHeadFromBoardRotation;
            _localLeftHandFromBoardRotation = localLeftHandFromBoardRotation;
            _localRigthHandFromBoardRotation = localRigthHandFromBoardRotation;
        }

        void FixedUpdate()
        {
            bool isCodeRunOnPlayerComputer = base.isLocalPlayer;
            if (isCodeRunOnPlayerComputer)
            {
                SendLocalComputedInformationToMirrorServer();
            }
            if (PlayerChessXrTagMono_ChessBoardCenter._in_scene_tag != null)
            {
                _head.localPosition = _localHeadFromBoardPosition;
                _leftHand.localPosition = _localLeftHandFromBoardPosition;
                _rightHand.localPosition = _localRigthHandFromBoardPosition;
                _head.localRotation = _localHeadFromBoardRotation;
                _leftHand.localRotation = _localLeftHandFromBoardRotation;
                _rightHand.localRotation = _localRigthHandFromBoardRotation;

                Transform chessBoard = PlayerChessXrTagMono_ChessBoardCenter._in_scene_tag;
                SetToWorld(_head, chessBoard, _localHeadFromBoardPosition, _localHeadFromBoardRotation);
                SetToWorld(_leftHand, chessBoard, _localLeftHandFromBoardPosition, _localLeftHandFromBoardRotation);
                SetToWorld(_rightHand, chessBoard, _localRigthHandFromBoardPosition, _localRigthHandFromBoardRotation);
            }
        }

        public static void GetLocal(Transform point, Transform reference, out Vector3 localPosition, out Quaternion localRotation)
        {
            GetWorldToLocal_DirectionalPoint(point.position, point.rotation, reference.position, reference.rotation, out localPosition, out localRotation);
        }
        public static void GetWorldToLocal_DirectionalPoint(in Vector3 worldPosition, in Quaternion worldRotation, in Vector3 positionReference, in Quaternion rotationReference, out Vector3 localPosition, out Quaternion localRotation)
        {
            localRotation = Quaternion.Inverse(rotationReference) * worldRotation;
            localPosition = Quaternion.Inverse(rotationReference) * (worldPosition - positionReference);
        }

        public static void SetToWorld(Transform point, Transform reference, in Vector3 localPosition, in Quaternion localRotation)
        {
            GetLocalToWorld_DirectionalPoint(localPosition, localRotation, reference.position, reference.rotation, out Vector3 worldPosition, out Quaternion worldRotation);
            point.position = worldPosition;
            point.rotation = worldRotation;
        }
        public static void GetLocalToWorld_DirectionalPoint(in Vector3 localPosition, in Quaternion localRotation, in Vector3 positionReference, in Quaternion rotationReference, out Vector3 worldPosition, out Quaternion worldRotation)
        {
            worldRotation = rotationReference * localRotation;
            worldPosition = (rotationReference * localPosition) + (positionReference);
        }
    }
}