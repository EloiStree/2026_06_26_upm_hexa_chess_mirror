using UnityEngine;

public class HexaChessMirrorMono_BoardStaticCommands : MonoBehaviour
{

    public void ClaimAndResetPiecePosition() {

        HexaChessMirrorMono_ClaimAuthorityOfMoving.S_ClaimThemAll();
        HexaChessMirrorMono_MoveToStartAnchor.S_ResetThemAllAtStart();

    }

    public void SaveOnLocalMachinePiecePosition() {
    
    }

    public void ReloadFromLocalMachinePiecePosition() { 
    
    }

}
