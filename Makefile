PLATFORM := macosx_x64
PROTOC := ./contrib/tools/$(PLATFORM)/protoc
PROTOBUF_OUT := ./AxGridToolsTest/Proto/
PROTOBUF_SRC := ./contrib/proto/


all: build_proto 

build_proto:
	mkdir -p $(PROTOBUF_OUT)
	$(PROTOC) --csharp_out=$(PROTOBUF_OUT) --proto_path=$(PROTOBUF_SRC) $(PROTOBUF_SRC)/*.proto


.PHONY: all build_proto 