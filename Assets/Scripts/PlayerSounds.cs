using UnityEngine;
using System.Collections;

public class PlayerSounds : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private LineConnector connector;
    [SerializeField] private AudioSource[] audioSource;
    [SerializeField] private AudioClip[] stepSound;
    [SerializeField] private AudioClip[] otherSounds;
    [SerializeField] private AudioClip[] musicSounds;

    [Header("Variables")]
    [SerializeField] private float fadeDuration = 2f;

    private int currentTrackIndex = 0;

    private bool isThrown;
    private bool isDash;
    private bool isJump;
    private bool isHurt;

    void Start ()
    {
        StartCoroutine(Music());
    }

    void Update()
    {
        FootStep();
        ThrowSound();
        DashSound();
        JumpSound();
    }

    private void FootStep()
    {
        if (Mathf.Abs(controller.horizontal) > 0.1f)
            CheckFloor();
        else
            audioSource[0].Pause();
    }

    private void CheckFloor()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * (controller.playerHalfHeight + 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, controller.playerHalfHeight + 0.1f);

        if (!hit) return;

        PhysicsMaterial2D mat = hit.collider.sharedMaterial;

        if (mat == null) return;

        if (mat.name == "Grass")
        {
            PlayFootstep(stepSound[0]);
        }
            
        else if (mat.name == "Stone")
        {
            PlayFootstep(stepSound[1]);
        }
    }

    private void PlayFootstep(AudioClip clip)
    {
        if (audioSource[0].isPlaying && audioSource[0].clip == clip)
            return;

        if (audioSource[0].isPlaying && audioSource[0].clip != clip)
            audioSource[0].Stop();

        audioSource[0].clip = clip;
        audioSource[0].Play();
    }

    private void ThrowSound()
    {
        if (connector.state == LineConnector.State.Extending && isThrown == false)
        {
            isThrown = true;
            audioSource[1].PlayOneShot(otherSounds[0]);
        }

        if (connector.state == LineConnector.State.Retracting || connector.state == LineConnector.State.Pulling)
            isThrown = false;
    }

    private void DashSound()
    {
        if (controller.isDashing == true && isDash == false)
        {
            isDash = true;
            audioSource[1].PlayOneShot(otherSounds[1]);
        }

        if (controller.isDashing == false)
            isDash = false;
    }

    private void JumpSound()
    {
        if (controller.jumpBufferTimer > 0 && controller.coyoteTimer > 0 && isJump == false)
        {
            isJump = true;
            audioSource[1].PlayOneShot(otherSounds[2]);
        }

        if (controller.GetIsGrounded())
            isJump = false;
    }

    public void HitSound()
    {
        if (!isHurt)
        {
            isHurt = true;
            audioSource[1].PlayOneShot(otherSounds[3]);
            StartCoroutine(ResetHurt());
        }
    }

    private IEnumerator ResetHurt()
    {
        yield return new WaitForSeconds(0.1f);
        isHurt = false;
    }

    IEnumerator Music()
    {
        while (true)
        {
            currentTrackIndex = currentTrackIndex % musicSounds.Length;

            audioSource[2].clip = musicSounds[currentTrackIndex];
            audioSource[2].volume = 1f;
            audioSource[2].Play();

            yield return new WaitForSeconds(audioSource[2].clip.length - fadeDuration);

            float _time = 0f;
            while (_time < fadeDuration)
            {
                _time += Time.deltaTime;
                audioSource[2].volume = Mathf.Lerp(1f, 0f, _time / fadeDuration);
                yield return null;
            }

            audioSource[2].Stop();

            currentTrackIndex++;
            if (currentTrackIndex >= musicSounds.Length)
            {
                currentTrackIndex = 0;
            }
        }
    }
}