from mlagents_envs.environment import UnityEnvironment
# This is a non-blocking call that only loads the environment.
env = UnityEnvironment(file_name="/home/julien/Documents/UnityMLAgents/ml-agents/Project/Assets/ML-Agents/Examples/3DBall", seed=1, side_channels=[])
# Start interacting with the evironment.
env.reset()
#behavior_names = env.behavior_spec.keys()