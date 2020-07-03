from keras.models import Sequential, load_model
import keras2onnx
import onnxruntime

keras_model = load_model("models/binary_crossentropy.h5")
print(keras_model.name)
onnx_model = keras2onnx.convert_keras(keras_model, keras_model.name)
keras2onnx.save_model(onnx_model, "models/binary_crossentropy.onnx")
