from neural_network.train import Trainer

trainer = Trainer("models/paper-relu-2.h5")
trainer.train()
trainer.save()
trainer.save_architecture()