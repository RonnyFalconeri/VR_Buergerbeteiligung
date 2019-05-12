using Photon.Pun;
using UnityEngine;

namespace VRRoom
{
    public class AvatarAnimation : MonoBehaviourPun, IPunObservable
    {
        private Animator animator;
        private int idle = 0;
        private int walk = 1;
        private int current = 0;
        private bool isMyAvatar = true;

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            if ( PhotonNetwork.IsConnected )
            {
                if ( photonView.IsMine )
                {
                    isMyAvatar = true;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Only control our own avatar
            if ( isMyAvatar )
            {
                if (Input.GetKey("w"))
                {
                    current = walk;
                    animator.SetInteger("state", walk);
                }
                else if (Input.GetKey("a"))
                {
                    current = walk;
                    animator.SetInteger("state", walk);
                }
                else if (Input.GetKey("s"))
                {
                    current = walk;
                    animator.SetInteger("state", walk);
                }
                else if (Input.GetKey("d"))
                {
                    current = walk;
                    animator.SetInteger("state", walk);
                }
                else
                {
                    current = idle;
                    animator.SetInteger("state", idle);
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(current);
            }
            else
            {
                int current = (int)stream.ReceiveNext();
                animator.SetInteger("state", current);
            }
        }
    }
}
