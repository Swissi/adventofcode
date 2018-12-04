lines = File.readlines('input.txt')
cloth = {}
claims = 0
lastid = ""

def gen_hash_key(l, w, x, y)
  (x + w).to_s + '_' + (y + l).to_s
end

def get_area(area)
  arr2 = area.split('x')
  w = Integer(arr2[0])
  l = Integer(arr2[1])
  [l, w]
end

def get_start_points(coord)
  arr1 = coord.split(',')
  x = Integer(arr1[0])
  y = Integer(arr1[1].chomp(':'))
  [x, y]
end

lines.each do |line|
  arr = line.split(' ')
  id = arr[0]
  coord = arr[2]
  area = arr[3]
  does_intersect = false

  x, y = get_start_points(coord)
  l, w = get_area(area)

  (1..w).each do |w|
    (1..l).each do |l|
      hash_key = gen_hash_key(l, w, x, y)

      if cloth[hash_key].nil?
        cloth[hash_key] = 1
      elsif cloth[hash_key] == 1
        claims += 1
        cloth[hash_key] += 1
        does_intersect = true
      else
        cloth[hash_key] += 1
        does_intersect = true
      end
    end
  end

  if does_intersect == false
    lastid = id
  end
end

puts lastid

lines.each do |line|
  arr = line.split(' ')
  id = arr[0]
  coord = arr[2]
  area = arr[3]
  does_intersect = false

  x, y = get_start_points(coord)
  l, w = get_area(area)

  (1..w).each do |w|
    (1..l).each do |l|
      hash_key = gen_hash_key(l, w, x, y)
      if cloth[hash_key] > 1
        does_intersect = true
        break
      end
    end
  end

  puts id if does_intersect == false
end


