from neural_network.train import Trainer

trainer = Trainer("models/batch-1.h5")
trainer.train()
trainer.save()
trainer.save_architecture()