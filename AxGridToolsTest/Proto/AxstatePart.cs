// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: axstate_part.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace AxGrid.Internal.Proto {

  /// <summary>Holder for reflection information generated from axstate_part.proto</summary>
  public static partial class AxstatePartReflection {

    #region Descriptor
    /// <summary>File descriptor for axstate_part.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static AxstatePartReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChJheHN0YXRlX3BhcnQucHJvdG8SF2NvbS5heGdyaWQuYXhzdGF0ZV90ZXN0",
            "IuEBCgpQU3ViU3RydWN0EgoKAmlkGAEgASgEEgwKBG5hbWUYAiABKAkSDAoE",
            "ZGF0YRgDIAEoDBI0CgdzdHJ1Y3RzGAQgAygLMiMuY29tLmF4Z3JpZC5heHN0",
            "YXRlX3Rlc3QuUFN1YlN0cnVjdBI8Cg9zdWJfc3RhdGVfZW51bXMYBSADKA4y",
            "Iy5jb20uYXhncmlkLmF4c3RhdGVfdGVzdC5QU3RhdGVFbnVtEjcKCnN1Yl9z",
            "dHJ1Y3QYBiABKAsyIy5jb20uYXhncmlkLmF4c3RhdGVfdGVzdC5QU3ViU3Ry",
            "dWN0KlcKClBTdGF0ZUVudW0SHAoYUF9TVEFURV9FTlVNX1VOU1BFQ0lGSUVE",
            "EAASEwoPUF9TVEFURV9FTlVNX09LEAESFgoSUF9TVEFURV9FTlVNX0VSUk9S",
            "EAJCMwoXY29tLmF4Z3JpZC5heHN0YXRlX3Rlc3RQAaoCFUF4R3JpZC5JbnRl",
            "cm5hbC5Qcm90b2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::AxGrid.Internal.Proto.PStateEnum), }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::AxGrid.Internal.Proto.PSubStruct), global::AxGrid.Internal.Proto.PSubStruct.Parser, new[]{ "Id", "Name", "Data", "Structs", "SubStateEnums", "SubStruct" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum PStateEnum {
    [pbr::OriginalName("P_STATE_ENUM_UNSPECIFIED")] Unspecified = 0,
    [pbr::OriginalName("P_STATE_ENUM_OK")] Ok = 1,
    [pbr::OriginalName("P_STATE_ENUM_ERROR")] Error = 2,
  }

  #endregion

  #region Messages
  public sealed partial class PSubStruct : pb::IMessage<PSubStruct> {
    private static readonly pb::MessageParser<PSubStruct> _parser = new pb::MessageParser<PSubStruct>(() => new PSubStruct());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<PSubStruct> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::AxGrid.Internal.Proto.AxstatePartReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PSubStruct() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PSubStruct(PSubStruct other) : this() {
      id_ = other.id_;
      name_ = other.name_;
      data_ = other.data_;
      structs_ = other.structs_.Clone();
      subStateEnums_ = other.subStateEnums_.Clone();
      SubStruct = other.subStruct_ != null ? other.SubStruct.Clone() : null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PSubStruct Clone() {
      return new PSubStruct(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private ulong id_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ulong Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 2;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "data" field.</summary>
    public const int DataFieldNumber = 3;
    private pb::ByteString data_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Data {
      get { return data_; }
      set {
        data_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "structs" field.</summary>
    public const int StructsFieldNumber = 4;
    private static readonly pb::FieldCodec<global::AxGrid.Internal.Proto.PSubStruct> _repeated_structs_codec
        = pb::FieldCodec.ForMessage(34, global::AxGrid.Internal.Proto.PSubStruct.Parser);
    private readonly pbc::RepeatedField<global::AxGrid.Internal.Proto.PSubStruct> structs_ = new pbc::RepeatedField<global::AxGrid.Internal.Proto.PSubStruct>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::AxGrid.Internal.Proto.PSubStruct> Structs {
      get { return structs_; }
    }

    /// <summary>Field number for the "sub_state_enums" field.</summary>
    public const int SubStateEnumsFieldNumber = 5;
    private static readonly pb::FieldCodec<global::AxGrid.Internal.Proto.PStateEnum> _repeated_subStateEnums_codec
        = pb::FieldCodec.ForEnum(42, x => (int) x, x => (global::AxGrid.Internal.Proto.PStateEnum) x);
    private readonly pbc::RepeatedField<global::AxGrid.Internal.Proto.PStateEnum> subStateEnums_ = new pbc::RepeatedField<global::AxGrid.Internal.Proto.PStateEnum>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::AxGrid.Internal.Proto.PStateEnum> SubStateEnums {
      get { return subStateEnums_; }
    }

    /// <summary>Field number for the "sub_struct" field.</summary>
    public const int SubStructFieldNumber = 6;
    private global::AxGrid.Internal.Proto.PSubStruct subStruct_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::AxGrid.Internal.Proto.PSubStruct SubStruct {
      get { return subStruct_; }
      set {
        subStruct_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as PSubStruct);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(PSubStruct other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (Name != other.Name) return false;
      if (Data != other.Data) return false;
      if(!structs_.Equals(other.structs_)) return false;
      if(!subStateEnums_.Equals(other.subStateEnums_)) return false;
      if (!object.Equals(SubStruct, other.SubStruct)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0UL) hash ^= Id.GetHashCode();
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Data.Length != 0) hash ^= Data.GetHashCode();
      hash ^= structs_.GetHashCode();
      hash ^= subStateEnums_.GetHashCode();
      if (subStruct_ != null) hash ^= SubStruct.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Id != 0UL) {
        output.WriteRawTag(8);
        output.WriteUInt64(Id);
      }
      if (Name.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Name);
      }
      if (Data.Length != 0) {
        output.WriteRawTag(26);
        output.WriteBytes(Data);
      }
      structs_.WriteTo(output, _repeated_structs_codec);
      subStateEnums_.WriteTo(output, _repeated_subStateEnums_codec);
      if (subStruct_ != null) {
        output.WriteRawTag(50);
        output.WriteMessage(SubStruct);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id != 0UL) {
        size += 1 + pb::CodedOutputStream.ComputeUInt64Size(Id);
      }
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (Data.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Data);
      }
      size += structs_.CalculateSize(_repeated_structs_codec);
      size += subStateEnums_.CalculateSize(_repeated_subStateEnums_codec);
      if (subStruct_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(SubStruct);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(PSubStruct other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0UL) {
        Id = other.Id;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.Data.Length != 0) {
        Data = other.Data;
      }
      structs_.Add(other.structs_);
      subStateEnums_.Add(other.subStateEnums_);
      if (other.subStruct_ != null) {
        if (subStruct_ == null) {
          subStruct_ = new global::AxGrid.Internal.Proto.PSubStruct();
        }
        SubStruct.MergeFrom(other.SubStruct);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 8: {
            Id = input.ReadUInt64();
            break;
          }
          case 18: {
            Name = input.ReadString();
            break;
          }
          case 26: {
            Data = input.ReadBytes();
            break;
          }
          case 34: {
            structs_.AddEntriesFrom(input, _repeated_structs_codec);
            break;
          }
          case 42:
          case 40: {
            subStateEnums_.AddEntriesFrom(input, _repeated_subStateEnums_codec);
            break;
          }
          case 50: {
            if (subStruct_ == null) {
              subStruct_ = new global::AxGrid.Internal.Proto.PSubStruct();
            }
            input.ReadMessage(subStruct_);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
