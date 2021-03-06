﻿using Assets.Scripts.Utils;
using Assets.Systems.RoomRotationSystem.Components;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterShooting : MonoBehaviour {

        void Update () {

            Debug.DrawRay(transform.position, transform.forward * 5, Color.green);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                var wall = hit.collider.GetComponent<RoomRotationWallComponent>();
                if (wall)
                {
                    wall.ResetWallmarks();
                    wall.Mark();
                    if (KeyCode.Mouse0.WasReleased())
                    {
                        wall.RotateToThis();
                    }
                }
            }
        }
    }
}
