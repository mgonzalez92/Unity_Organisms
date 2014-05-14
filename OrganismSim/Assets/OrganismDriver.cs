using UnityEngine;
using System.Collections.Generic;
using System;

public class OrganismDriver : MonoBehaviour {
	public const int HUNGERSTART = 50;
	public const int STARVING = 15;
	public const int FULL = 85;
	public const int INPUTNUM = 4;
	public const int HIDDENNUM = 6;
	public const int OUTPUTNUM = 4;
	public const int MAXVELOCITY = 5;

	public int fitness = 0;
	public int hunger = HUNGERSTART;
	public float rotation = 0;
	public NeuronLayer[] neuronLayers = new NeuronLayer[3];
	public float[] input;
	public float[] hidden;
	public float[] output;

	// Use this for initialization
	void Start () {
		input = new float[INPUTNUM];
		hidden = new float[HIDDENNUM];
		output = new float[OUTPUTNUM];

		//Neuron layers
		neuronLayers[0] = CreateNeuronLayer(INPUTNUM, 0);
		neuronLayers[1] = CreateNeuronLayer(HIDDENNUM, INPUTNUM);
		neuronLayers[2] = CreateNeuronLayer(OUTPUTNUM, HIDDENNUM);

		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < neuronLayers[i].neuronNum; j++)
			{
				for (int k = 0; k < neuronLayers[i].neurons[j].inputNum + 1; k++)
				{	
					neuronLayers[i].neurons[j].weights[k] = UnityEngine.Random.Range(0.0f, 1000.0f) / 1000.0f;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Calculate output through the network
		//ComputeNetwork();
		
		//Update location and screen
		//UpdateLocation();
	}
	public void ComputeNetwork(Vector3 foodPos)
	{
		int i = 0;
		int j = 0;
		
		//Inputs
		input [0] = transform.position.x;
		input [1] = transform.position.y;
		input [2] = foodPos.x;
		input [3] = foodPos.y;
		
		//Calculate values at hidden nodes
		for (i = 0; i < HIDDENNUM; i++)
		{
			hidden[i] = 0;
			for (j = 0; j < INPUTNUM; j++)
			{
				hidden[i] += input[j] * neuronLayers[1].neurons[i].weights[j];
			}
			hidden[i] += neuronLayers[1].neurons[i].weights[INPUTNUM];
			hidden[i] = (1 / (float)(1 + Math.Pow(Math.E, -hidden[i])));
		}
		
		//Calculate outputs
		for (i = 0; i < OUTPUTNUM; i++)
		{
			output[i] = 0;
			for (j = 0; j < HIDDENNUM; j++)
			{
				output[i] += hidden[j] * neuronLayers[2].neurons[i].weights[j];
			}
			output[i] += neuronLayers[2].neurons[i].weights[HIDDENNUM];
			output[i] = (1 / (float)(1 + Math.Pow(Math.E, -output[i])));
		}
	}
	
	public void UpdateLocation()
	{
		//Rotation
		rotation += output[0] - output[1];
		
		//Movement
		float x = (float) Math.Cos(rotation) * (output[2] - output[3]) * MAXVELOCITY;
		float y = (float) Math.Sin(rotation) * (output[2] - output[3]) * MAXVELOCITY;
		
		//Food
		hunger--;
		//if collision with food { hunger += FOOD }
		if (hunger < 0) { /*Dead*/ }
		if (hunger > 100) { /*Dead*/ }
		
		transform.position += Vector3.up * y * Time.deltaTime + Vector3.right * x * Time.deltaTime;
		transform.Rotate (0, 0, rotation);
		//Update screen
		//To do: Rotation = rotation
		//To do: Transform += (x, y)
		//To do: Color = output[4-7]
	}
	
	NeuronLayer CreateNeuronLayer(int neuronNum, int inputNum)
	{
		NeuronLayer neuronLayer = new NeuronLayer();
		neuronLayer.neuronNum = neuronNum;
		neuronLayer.neurons = new Neuron[neuronNum];
		for (int i = 0; i < neuronNum; i++)
		{
			neuronLayer.neurons[i] = CreateNeuron(inputNum);
		}
		return neuronLayer;
	}
	
	Neuron CreateNeuron(int inputNum)
	{
		Neuron neuron = new Neuron();
		neuron.inputNum = inputNum;
		neuron.weights = new float[inputNum + 1];
		return neuron;
	}
	void OnTriggerEnter2D ( Collider2D col) {
		GameObject collision = col.gameObject;
		if (collision.name == "Food(Clone)") {
			Debug.Log ("Food Collision!");
			collision.transform.position = new Vector3(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range (-50, 50), 0);
			fitness+=1;
		}
	}
}

public struct Neuron
{
	public int inputNum;
	public float[] weights;
}

public struct NeuronLayer
{
	public int neuronNum;
	public Neuron[] neurons;
}