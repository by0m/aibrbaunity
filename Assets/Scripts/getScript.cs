using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Video;
using Cinemachine;

public class getScript : MonoBehaviour
{
    private string url = "http://127.0.0.1:5000/";
    public AudioSource audioSource;
    public TextMeshProUGUI subtitleText;
    public characterController characterController;

    public VideoPlayer player;
    public GameObject videoImage;
    public AudioSource backgroundMusic;

    public TextMeshProUGUI topicText;

    [SerializeField]
    GameObject noScriptScreen;

    [SerializeField]
    CinemachineVirtualCamera camera;


    string tips;

    void Start()
    {
        StartCoroutine(GetRequest(url));
    }
    private IEnumerator GetRequest(string url)
    {
        
        
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            string response = webRequest.downloadHandler.text;

            

            if(response.Trim() == "null" || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                subtitleText.text = "";
                topicText.text = "";
                Debug.Log("No scripts found");
                noScriptScreen.SetActive(true);

                camera.Follow = null;
                camera.LookAt = null;

                camera.transform.position = new Vector3(-142.56f, 79.73f, 25.37f);
                camera.transform.rotation = Quaternion.Euler(24.964f, -142.845f, 1.754f);
                yield return new WaitForSeconds(1);
            }
            else
            {
                
                Debug.Log("Found scripts");


                characterController.reset(true);
                noScriptScreen.SetActive(false);
                topicText.text = "";
                backgroundMusic.volume = 0;
                player.Play();
                videoImage.SetActive(true);

                yield return new WaitForSeconds((float)player.clip.length);

                backgroundMusic.volume = 0.045f;
                videoImage.SetActive(false);

                Debug.Log("response = " + response);

                Data[] data = JsonConvert.DeserializeObject<Data[]>(response);


                List<string> speakers = new List<string>();
                foreach (Data d in data)
                {
                    speakers.Add(d.name);
                    Debug.Log(d.name);
                    tips = d.donations;
                    topicText.text = "Current topic: \"" + d.topic + "\" suggested by: " + d.author;
                }


                GameObject[] paidCharacters = GameObject.FindGameObjectsWithTag("paidCharacter");

                foreach(GameObject character in paidCharacters)
                {
                    Destroy(character);
                }

                int index = 0;

                foreach (string tip in tips.Split("uibImKsUsR"))
                {
                    if(index != 0)
                    {
                        name = "Anonymous";
                        if (tip.Trim() != "")
                        {
                            name = tip;
                        }
                        characterController.summonHank(name);
                    }

                    index += 1;
                    
                }

                

                

                int speakerIndex = 0;
                foreach (Data d in data)
                {
                    if(speakers.Count - speakerIndex > 1)
                    {
                        characterController.stopMoving(speakers[speakerIndex]);

                        characterController.stopMoving(speakers[speakerIndex + 1]);

                        characterController.lookAtWithMove(speakers[speakerIndex + 1], speakers[speakerIndex]);

                        characterController.lookAt(speakers[speakerIndex], speakers[speakerIndex + 1]);
                    }
                        
                    if (speakers.Count > speakerIndex + 2)
                    {
                        characterController.stopMoving(speakers[speakerIndex + 2]);

                        characterController.lookAtWithMove(speakers[speakerIndex + 2], speakers[speakerIndex + 1]);
                    }


                    characterController.switchCamera(d.name);
                    subtitleText.text = d.text;
                    PlayAudioFile(d.audio);
                    yield return new WaitForSeconds(audioSource.clip.length);

                    characterController.startMoving(d.name);

                    if (speakers.Count > speakerIndex + 2)
                    {
                        characterController.startMoving(speakers[speakerIndex + 2]);
                    }


                    speakerIndex++;
                }
                    
            }


            yield return new WaitForSeconds(2);
            characterController.reset(false);
            StartCoroutine(GetRequest(url));
            
            
        }
    }



    //this bit is all bing idk how it works
    public void PlayAudioFile(string path)
    {
        if (File.Exists(path)) // check if the file exists
        {
            // load the audio file as a byte array
            byte[] audioData = File.ReadAllBytes(path);

            // get the sample rate and channel count from the WAV header
            int sampleRate = BitConverter.ToInt32(audioData, 24);
            int channels = BitConverter.ToInt16(audioData, 22);

            // get the data size and offset from the WAV header
            int dataSize = BitConverter.ToInt32(audioData, 40);
            int dataOffset = 44;

            // create a float array for the audio samples
            float[] samples = new float[dataSize / 2];

            // convert the byte array to a float array
            for (int i = 0; i < samples.Length; i++)
            {
                // get the 16-bit sample as a short
                short sample = BitConverter.ToInt16(audioData, dataOffset + i * 2);

                // normalize the sample to the range [-1, 1]
                samples[i] = sample / 32768f;
            }

            // create an audio clip from the float array
            AudioClip audioClip = AudioClip.Create("AudioClip", samples.Length, channels, sampleRate, false);

            // set the audio data of the clip
            audioClip.SetData(samples, 0);

            // play the audio clip from the audio source
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("File not found: " + path);
        }
    }

}

// define a class that matches the structure of your JSON object
public class Data
{
    public string audio { get; set; }
    public string name { get; set; }
    public string text { get; set; }
    public string topic { get; set; }
    public string author { get; set; }
    public string donations { get; set; }
}
