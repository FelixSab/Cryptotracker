
using System.ComponentModel.DataAnnotations;

namespace Cryptotracker.Contracts.DTOs;

public record ChatCompletionRequestDto(

    [Required]
    string? UserMessage,

    bool IgnoreChatHistory,
    bool IsAdminChat,
    bool IsTraceLogEnabled
);
