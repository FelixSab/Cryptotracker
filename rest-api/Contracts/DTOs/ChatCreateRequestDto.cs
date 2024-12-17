using System.ComponentModel.DataAnnotations;

namespace Cryptotracker.Contracts.DTOs;

public record ChatCreateRequestDto(

    [Required]
    string? BotId

);
