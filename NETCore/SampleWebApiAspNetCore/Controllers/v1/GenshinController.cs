using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Services;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using System.Text.Json;

namespace SampleWebApiAspNetCore.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GenshinController : ControllerBase
    {
        private readonly IGenshinRepository _genshinRepository;
        private readonly IMapper _mapper;
        private readonly ILinkService<GenshinController> _linkService;

        public GenshinController(
            IGenshinRepository genshinRepository,
            IMapper mapper,
            ILinkService<GenshinController> linkService)
        {
            _genshinRepository = genshinRepository;
            _mapper = mapper;
            _linkService = linkService;
        }

        [HttpGet(Name = nameof(GetAllGenshin))]
        public ActionResult GetAllGenshin(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<GenshinEntity> genItems = _genshinRepository.GetAll(queryParameters).ToList();

            var allItemCount = _genshinRepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = _linkService.CreateLinksForCollection(queryParameters, allItemCount, version);
            var toReturn = genItems.Select(x => _linkService.ExpandSingleGenChar(x, x.Id, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleChar))]
        public ActionResult GetSingleChar(ApiVersion version, int id)
        {
            GenshinEntity genItem = _genshinRepository.GetSingle(id);

            if (genItem == null)
            {
                return NotFound();
            }

            GenDto item = _mapper.Map<GenDto>(genItem);

            return Ok(_linkService.ExpandSingleGenChar(item, item.Id, version));
        }

        [HttpPost(Name = nameof(AddGenChar))]
        public ActionResult<GenDto> AddGenChar(ApiVersion version, [FromBody] GenCreateDto genCreateDto)
        {
            if (genCreateDto == null)
            {
                return BadRequest();
            }

            GenshinEntity toAdd = _mapper.Map<GenshinEntity>(genCreateDto);

            _genshinRepository.Add(toAdd);

            if (!_genshinRepository.Save())
            {
                throw new Exception("Creating a genItem failed on save.");
            }

            GenshinEntity newgenChar = _genshinRepository.GetSingle(toAdd.Id);
            GenDto GenDto = _mapper.Map<GenDto>(newgenChar);

            return CreatedAtRoute(nameof(GetSingleChar),
                new { version = version.ToString(), id = newgenChar.Id },
                _linkService.ExpandSingleGenChar(GenDto, GenDto.Id, version));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdatedGenChar))]
        public ActionResult<GenDto> PartiallyUpdatedGenChar(ApiVersion version, int id, [FromBody] JsonPatchDocument<GenUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            GenshinEntity existingEntity = _genshinRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            GenUpdateDto GenUpdateDto = _mapper.Map<GenUpdateDto>(existingEntity);
            patchDoc.ApplyTo(GenUpdateDto);

            TryValidateModel(GenUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(GenUpdateDto, existingEntity);
            GenshinEntity updated = _genshinRepository.Update(id, existingEntity);

            if (!_genshinRepository.Save())
            {
                throw new Exception("Updating a genItem failed on save.");
            }

            GenDto GenDto = _mapper.Map<GenDto>(updated);

            return Ok(_linkService.ExpandSingleGenChar(GenDto, GenDto.Id, version));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveGenChar))]
        public ActionResult RemoveGenChar(int id)
        {
            GenshinEntity genItem = _genshinRepository.GetSingle(id);

            if (genItem == null)
            {
                return NotFound();
            }

            _genshinRepository.Delete(id);

            if (!_genshinRepository.Save())
            {
                throw new Exception("Deleting a genItem failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateGenChar))]
        public ActionResult<GenDto> UpdateGenChar(ApiVersion version, int id, [FromBody] GenUpdateDto GenUpdateDto)
        {
            if (GenUpdateDto == null)
            {
                return BadRequest();
            }

            var existinggenItem = _genshinRepository.GetSingle(id);

            if (existinggenItem == null)
            {
                return NotFound();
            }

            _mapper.Map(GenUpdateDto, existinggenItem);

            _genshinRepository.Update(id, existinggenItem);

            if (!_genshinRepository.Save())
            {
                throw new Exception("Updating a genItem failed on save.");
            }

            GenDto GenDto = _mapper.Map<GenDto>(existinggenItem);

            return Ok(_linkService.ExpandSingleGenChar(GenDto, GenDto.Id, version));
        }

        [HttpGet("GetRandomGenChar", Name = nameof(GetRandomGenChar))]
        public ActionResult GetRandomGenChar()
        {
            ICollection<GenshinEntity> genItems = _genshinRepository.GetRandomGenChar();

            IEnumerable<GenDto> dtos = genItems.Select(x => _mapper.Map<GenDto>(x));

            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(Url.Link(nameof(GetRandomGenChar), null), "self", "GET"));

            return Ok(new
            {
                value = dtos,
                links = links
            });
        }
    }
}
