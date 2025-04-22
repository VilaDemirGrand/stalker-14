import os
import yaml
import copy

class CustomLoader(yaml.SafeLoader):
  pass

def type_constructor(loader, tag_suffix, node):
  tag_type = tag_suffix.split(':')[-1]

  if isinstance(node, yaml.MappingNode):
    value = loader.construct_mapping(node)
    value['type'] = tag_type
    return value

  if isinstance(node, yaml.ScalarNode):
    return {'type': tag_type}

  raise yaml.constructor.ConstructorError(
    None,
    None,
    f"Unsupported YAML node type: {type(node)} with tag !type:{tag_type}",
    node.start_mark
  )

CustomLoader.add_multi_constructor('!type:', type_constructor)

def load_yml_prototypes_recursive(directory):
  prototypes = []

  for root, _, files in os.walk(directory):
    for file in files:
      if not file.endswith('.yml'):
        continue

      full_path = os.path.join(root, file)

      try:
        with open(full_path, 'r', encoding='utf-8') as f:
          content = yaml.load(f, Loader = CustomLoader)
          if isinstance(content, list) or isinstance(content, dict):
            prototypes.extend(content)
      except Exception as e:
        print(f"⚠️ Ошибка при парсинге {full_path}: {e}")

  return prototypes

def merge_components(parent_comps, child_comps):
  result = []


  for comp in parent_comps:
    result.append(copy.deepcopy(comp))

  return result

def deep_merge_dicts(base, override):
  result = copy.deepcopy(base)
  for key, value in override.items():
    if isinstance(value, dict) and key in result and isinstance(result[key], dict):
      result[key] = deep_merge_dicts(result[key], value)
    else:
      result[key] = copy.deepcopy(value)
  return result

def resolve_inheritance(prototypes):
  id_to_proto = {proto['id']: proto for proto in prototypes if 'id' in proto}
  resolved = {}

  def resolve(proto):
    proto_id = proto.get('id')
    if proto_id in resolved:
      return resolved[proto_id]

    merged = {}

    # Массив или одиночный parent
    parents = proto.get('parent')
    if isinstance(parents, str):
      parents = [parents]
    elif not isinstance(parents, list):
      parents = []

    # Мержим всех родителей
    for parent_id in parents:
      if parent_id not in id_to_proto:
        print(f"⚠️ Предупреждение: не найден родитель '{parent_id}' для '{proto_id}'")
        continue
      parent_proto = resolve(id_to_proto[parent_id])
      merged = deep_merge_dicts(merged, parent_proto)

    # Накладываем текущий поверх
    merged = deep_merge_dicts(merged, proto)

    # Компоненты обрабатываются отдельно
    parent_comps = merged.get('components', [])
    child_comps = proto.get('components', [])
    merged['components'] = merge_components(parent_comps, child_comps)

    resolved[proto_id] = merged
    return merged

  return [resolve(proto) for proto in prototypes if 'id' in proto]

# 🎯 Пример использования
if __name__ == "__main__":
  path_to_prototypes = "./Guns/"
  all_prototypes = load_yml_prototypes_recursive(path_to_prototypes)
  all_prototypes = resolve_inheritance(all_prototypes)

  print(f"Итоговое количество прототипов: {len(all_prototypes)}")
  print('Id;Name;Description;Abstract;Parent;FireRate;MinAngle;MaxAngle;ProjectileSpeed;AngleIncrease;AccuracyMultiplier;AccuracyMultiplierUnwielded;FalloffMultiplier')

  for proto in all_prototypes:
    components = {item['type']: {k: v for k, v in item.items() if k != 'type'} for item in proto.get('components')}

    if 'Gun' not in components:
      continue

    gunComponent = components['Gun']
    accuracyComponent = components.get('STWeaponAccuracy', {})
    falloffComponent = components.get('STWeaponDamageFalloff', {})

    fireRate = gunComponent.get('fireRate', 0)
    minAngle = gunComponent.get('minAngle', 0)
    maxAngle = gunComponent.get('maxAngle', 0)
    projectileSpeed = gunComponent.get('projectileSpeed', 0)
    angleIncrease = gunComponent.get('angleIncrease', 0)

    accuracyMultiplier = accuracyComponent.get('accuracyMultiplier', 1)
    accuracyMultiplierUnwielded = accuracyComponent.get('accuracyMultiplierUnwielded', 1)

    falloffMultiplier = falloffComponent.get('falloffMultiplier', 1)

    id = proto.get('id', '-')
    name = proto.get('name', '-')
    description = proto.get('description', '-')
    abstract = proto.get('abstract', False)
    parent = proto.get('parent', '-')
    print(f'{id};{name};{description};{abstract};{parent};{fireRate};{minAngle};{maxAngle};{projectileSpeed};{angleIncrease};{accuracyMultiplier};{accuracyMultiplierUnwielded};{falloffMultiplier}')
