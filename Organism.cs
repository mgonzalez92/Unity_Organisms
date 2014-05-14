using System;

class Organism
{
    public const int HUNGERSTART = 50;
    public const int STARVING = 15;
    public const int FULL = 85;
    public const int INPUTNUM = 16;
    public const int HIDDENNUM = 11;
    public const int OUTPUTNUM = 7;
    public const int MAXVELOCITY = 5;

    public int fitness = 0;
    public int hunger = HUNGERSTART;
    public int rotation = 0;
    public NeuronLayer[] neuronLayers = new NeuronLayer[3];
    public float input = new int[INPUTNUM];
    public float hidden = new int[HIDDENNUM];
    public float output = new int[OUTPUTNUM];

    public Organism() //This is the init()
    {
        //Neuron layers
        neuronLayers[0] = CreateNeuronLayer(INPUTNUM, 0);
        neuronLayers[1] = CreateNeuronLayer(HIDDENNUM, INPUTNUM);
        neuronLayers[2] = CreateNeuronLayer(OUTPUTNUM, HIDDENNUM);
    }

    public void Update()
    {
        //Calculate output through the network
        ComputeNetwork();

        //Update location and screen
        UpdateLocation();
    }

    public void ComputeNetwork()
    {
        int i = 0;
        int j = 0;

        //Inputs
        for (i = 0; i < INPUTNUM; i++)
        {
            input[i] = -1; //Don't know how we'll do this
        }

        //Calculate values at hidden nodes
        for (i = 0; i < HIDDENNUM; i++)
        {
            hidden[i] = 0;
            for (j = 0; j < INPUTNUM; j++)
            {
                hidden[i] += input[j] * neuronLayers[1].weights[j];
            }
            hidden[i] += neuronLayers[1].weights[INPUTNUM];
            hidden[i] = (1 / (float)(1 + Math.Pow(Math.E, -hidden[i])));
        }

        //Calculate outputs
        for (i = 0; i < OUTPUTNUM; i++)
        {
            output[i] = 0;
            for (j = 0; j < HIDDENNUM; j++)
            {
                output[i] += hidden[j] * neuronLayers[2].weights[j];
            }
            output[i] += neuronLayers[2].weights[HIDDENNUM];
            output[i] = (1 / (float)(1 + Math.Pow(Math.E, -output[i])));
        }
    }

    public void UpdateLocation()
    {
        //Rotation
        rotation += output[0] - output[1];

        //Movement
        int x = Math.Cos(rotation) * (output[3] - output[4]) * MAXVELOCITY;
        int y = Math.Sin(rotation) * (output[3] - output[4]) * MAXVELOCITY;

        //Food
        hunger--;
        //if collision with food { hunger += FOOD }
        if (hunger < 0) { /*Dead*/ }
        if (hunger > 100) { /*Dead*/ }

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
}

struct Neuron
{
    public int inputNum;
    public float[] weights;
}

struct NeuronLayer
{
    public int neuronNum;
    public Neuron[] neurons;
}