//using Mirror;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;
//using System.Linq;
//using System.Runtime.CompilerServices;



//public class HexaChessMirrorMono_RsaKeyIdentity : NetworkBehaviour
//{

//    static HexaChessMirrorMono_RsaKeyIdentity m_ownedIdentity;
//    public static void GetOwnedRsaIdentity(out HexaChessMirrorMono_RsaKeyIdentity owned)
//    {
//        owned = m_ownedIdentity;
//    }
//    public static void GetOwnedMirrorIdentity(out NetworkIdentity owned)
//    {
//        owned = m_ownedIdentity?.GetComponent<NetworkIdentity>();
//    }

//    [SyncVar]
//    public string m_publicXmlKey;

//    [SyncVar(hook = nameof(NotifyPlayerProvedIdentityRight))]
//    public bool m_isSignatureValid;

//    public MirrorRsaPlayerOnNetworkRef m_playerRef;


//    public UnityEvent<string> m_onPlayerProvedIdentity;

//    public AbstractKeyPairRsaHolderMono m_clientFetchKey;


//    public void NotifyPlayerProvedIdentityRight(bool _, bool newValue)
//    {
//        if (newValue)
//        {
//            // MirrorRsaPlayerOnNetworkRefDico.InstanceInScene.RemovePlayerNotValide();
//            m_playerRef = new MirrorRsaPlayerOnNetworkRef(this, m_publicXmlKey);
//            m_onPlayerProvedIdentity.Invoke(m_publicXmlKey);
//        }
//        else
//        {
//            m_playerRef = null;
//        }
//    }


//    public Debug m_debug = new Debug();
//    [System.Serializable]
//    public class Debug
//    {
//        public string m_messageToSign;
//        public byte[] m_messageToSignAsByte;
//        public byte[] m_signedByte;
//        public bool m_isOnHost;
//        public bool m_isClientAndOwned;
//    }

//    public bool IsSignedValidatedByServer()
//    {
//        return m_isSignatureValid;
//    }
//    public override void OnStartClient()
//    {
//        m_debug.m_isOnHost = isServer;
//        m_debug.m_isClientAndOwned = isClient && isOwned;
//        if (m_debug.m_isClientAndOwned)
//        {
//            m_ownedIdentity = this;
//            ReloadThePublicKeyAndStartToSignIt();
//        }
//    }

//    [ContextMenu("Reload the public key and sign it")]
//    public void ReloadThePublicKeyAndStartToSignIt()
//    {
//        if (m_debug.m_isClientAndOwned)
//        {

//            m_clientFetchKey.GetPublicXml(out string publicKey);
//            CmdPushPublicKeyToServer(publicKey);
//        }
//    }

//    [Command]
//    void CmdPushPublicKeyToServer(string pubicKey)
//    {
//        m_publicXmlKey = pubicKey;
//        m_debug.m_messageToSign = Guid.NewGuid().ToString();
//        m_debug.m_messageToSignAsByte = System.Text.Encoding.UTF8.GetBytes(m_debug.m_messageToSign);
//        RpcPushMessageToSign(m_debug.m_messageToSignAsByte);
//    }

//    [ServerCallback]
//    [TargetRpc]
//    void RpcPushMessageToSign(byte[] byteMessageToSign)
//    {
//        m_debug.m_messageToSignAsByte = byteMessageToSign;
//        m_debug.m_messageToSign = System.Text.Encoding.UTF8.GetString(byteMessageToSign);
//        print("Hello " + m_debug.m_messageToSign);
//        byte[] signedByte = KeyPairRsaHolderToSignMessageUtility.SignData(m_debug.m_messageToSignAsByte, m_clientFetchKey);
//        m_debug.m_signedByte = signedByte;
//        CmdPushSignedMessage(signedByte);
//    }


//    [Command]
//    void CmdPushSignedMessage(byte[] signedMessage)
//    {
//        m_debug.m_signedByte = signedMessage;
//        m_isSignatureValid = KeyPairRsaHolderToSignMessageUtility.VerifySignature(m_debug.m_messageToSignAsByte, m_debug.m_signedByte, m_publicXmlKey);

//        if (m_isSignatureValid)
//        {
//            print("Signature is valid");
//        }
//        else
//        {
//            print("Signature is invalid.");
//        }
//    }

//    public MirrorRsaPlayerOnNetworkRef GetRsaRef()
//    {
//        return m_playerRef;
//    }

//    public static bool IsInstanciated()
//    {
//        return m_ownedIdentity != null;
//    }
//    public static bool IsInstanciatedAndConnected()
//    {
//        if (m_ownedIdentity == null) return false;
//        GetOwnedRsaIdentity(out m_ownedIdentity);
//        return m_ownedIdentity.netId != 0;

//    }

//    public static bool IsInstanciatedConnectedAndSigned()
//    {
//        if (m_ownedIdentity == null) return false;
//        GetOwnedRsaIdentity(out m_ownedIdentity);
//        return m_ownedIdentity.netId != 0 && m_ownedIdentity.m_isSignatureValid;
//    }
//}